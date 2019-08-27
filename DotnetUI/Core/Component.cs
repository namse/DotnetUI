namespace DotnetUI.Core
{
    public class UserDefinedComponent
    {

    }
    public abstract class PlatformSpecificComponent<TProps>
        : Component<TProps>
         where TProps : DefaultComponentProps
    {
        protected PlatformSpecificComponent(TProps props) : base(props)
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
        protected Component(DefaultComponentProps props)
        {
            Props = props;
        }

        public abstract Blueprint Render();
    }

    public abstract class Component<TProps>
        : Component, IComponentWithProps<TProps>
        where TProps : DefaultComponentProps
    {
        protected Component(TProps props) : base(props)
        {
        }
    }

    public abstract class Component<TProps, TState>
        : Component<TProps>
        where TProps : DefaultComponentProps
        where TState : DefaultComponentState
    {
        protected TProps props;
        protected TState state;

        protected Component(TProps props) : base(props)
        {
            this.props = props;
        }

        protected void CommitState()
        {

        }

    }

}