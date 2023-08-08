using System;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using System.Windows.Markup;

namespace SteeringWheel.Service.Buttons
{
    [MarkupExtensionReturnType(typeof(ICommand))]
    public class CommandBindingExtension : MarkupExtension
    {
        public CommandBindingExtension()
        {
        }

        public CommandBindingExtension(string commandName)
        {
            this.CommandName = commandName;
        }

        [ConstructorArgument("commandName")]
        public string CommandName { get; set; }

        private object targetObject;
        private object targetProperty;

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            IProvideValueTarget provideValueTarget = serviceProvider.GetService(typeof(IProvideValueTarget)) as IProvideValueTarget;
            if (provideValueTarget != null)
            {
                targetObject = provideValueTarget.TargetObject;
                targetProperty = provideValueTarget.TargetProperty;
            }

            if (!string.IsNullOrEmpty(CommandName))
            {
                ParserContext parserContext = GetPrivateFieldValue<ParserContext>(serviceProvider, "_context");
                if (parserContext != null)
                {
                    FrameworkElement rootElement = GetPrivateFieldValue<FrameworkElement>(parserContext, "_rootElement");
                    if (rootElement != null)
                    {
                        object dataContext = rootElement.DataContext;

                        if (!dataContextChangeHandlerSet)
                        {
                            rootElement.DataContextChanged += new DependencyPropertyChangedEventHandler(rootElement_DataContextChanged);
                            dataContextChangeHandlerSet = true;
                        }

                        if (dataContext != null)
                        {
                            ICommand command = GetCommand(dataContext, CommandName);
                            if (command != null)
                                return command;
                        }
                    }
                }
            }
            return ActionCommand.Instance;
        }

        private ICommand GetCommand(object dataContext, string commandName)
        {
            PropertyInfo prop = dataContext.GetType().GetProperty(commandName);
            if (prop != null)
            {
                ICommand command = prop.GetValue(dataContext, null) as ICommand;
                if (command != null)
                    return command;
            }
            return null;
        }

        private void AssignCommand(ICommand command)
        {
            if (targetObject != null && targetProperty != null)
            {
                if (targetProperty is DependencyProperty)
                {
                    DependencyObject depObj = targetObject as DependencyObject;
                    DependencyProperty depProp = targetProperty as DependencyProperty;
                    depObj.SetValue(depProp, command);
                }
                else
                {
                    PropertyInfo prop = targetProperty as PropertyInfo;
                    prop.SetValue(targetObject, command, null);
                }
            }
        }

        private bool dataContextChangeHandlerSet = false;
        private void rootElement_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            FrameworkElement rootElement = sender as FrameworkElement;
            if (rootElement != null)
            {
                object dataContext = rootElement.DataContext;
                if (dataContext != null)
                {
                    ICommand command = GetCommand(dataContext, CommandName);
                    if (command != null)
                    {
                        AssignCommand(command);
                    }
                }
            }
        }

        private T GetPrivateFieldValue<T>(object target, string fieldName)
        {
            FieldInfo field = target.GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic);
            if (field != null)
            {
                return (T)field.GetValue(target);
            }
            return default(T);
        }
    }
}
