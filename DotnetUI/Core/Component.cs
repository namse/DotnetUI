namespace DotnetUI.Core
{
    public class UserDefinedComponent
    {

    }

    public interface IPlatformSpecificComponent
    {
    }

    public abstract class PlatformSpecificComponent<TProps>
        : Component<TProps>, IPlatformSpecificComponent
         where TProps : DefaultComponentProps
    {
        protected PlatformSpecificComponent(TProps props)
            : base(props)
        {
        }

        public sealed override Blueprint Render()
        {
            throw new System.NotImplementedException();
        }
    }
    public interface DefaultComponentProps
    {
        Blueprint[] Children { get; }
    }

    public class DefaultComponentState
    {

    }

    public interface IComponentWithProps<TProps> where TProps : DefaultComponentProps
    { }

    public abstract class Component
    {
        public readonly DefaultComponentProps Props;
        public IUpdater Updater;

        protected Component(DefaultComponentProps props)
        {
            Props = props;
        }

        public abstract Blueprint Render();
        
        protected void CommitState()
        {
            Updater.CommitUpdate(this);
        }
    }

    public abstract class Component<TProps>
        : Component, IComponentWithProps<TProps>
        where TProps : DefaultComponentProps
    {
        protected new readonly TProps Props;
        protected Component(TProps props) : base(props)
        {
            Props = props;
        }
    }

    public abstract class Component<TProps, TState>
        : Component<TProps>
        where TProps : DefaultComponentProps
        where TState : DefaultComponentState
    {
        protected abstract TState State { get; set; }

        protected Component(TProps props) : base(props)
        {
        }
    }

}