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
         where TProps : IDefaultComponentProps
    {
        protected PlatformSpecificComponent(TProps props)
            : base(props)
        {
        }

        public sealed override ComponentBlueprint Render()
        {
            throw new System.NotImplementedException();
        }
    }
    public interface IDefaultComponentProps
    {
        Blueprint[] Children { get; }
    }

    public class DefaultComponentState
    {

    }

    public interface IComponentWithProps<TProps> where TProps : IDefaultComponentProps
    { }

    public abstract class Component
    {
        public readonly IDefaultComponentProps Props;
        public IUpdater Updater;

        protected Component(IDefaultComponentProps props)
        {
            Props = props;
        }

        public abstract ComponentBlueprint Render();
        
        protected void CommitState()
        {
            Updater.CommitUpdate(this);
        }
    }

    public abstract class Component<TProps>
        : Component, IComponentWithProps<TProps>
        where TProps : IDefaultComponentProps
    {
        protected new readonly TProps Props;
        protected Component(TProps props) : base(props)
        {
            Props = props;
        }
    }

    public abstract class Component<TProps, TState>
        : Component<TProps>
        where TProps : IDefaultComponentProps
        where TState : DefaultComponentState
    {
        protected abstract TState State { get; set; }

        protected Component(TProps props) : base(props)
        {
        }
    }
}
