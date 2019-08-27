namespace DotnetUI
{

    public interface DefaultComponentProps
    {
        ReactElement[] Children { get; }
    }

    public class DefaultComponentState
    {

    }

    public interface IComponentWithProps<TProps> where TProps : DefaultComponentProps
    { }

    public abstract class ReactComponent
    {
        public readonly DefaultComponentProps Props;
        protected ReactComponent(DefaultComponentProps props)
        {
            Props = props;
        }

        public abstract ReactElement Render();
    }

    public abstract class ReactComponent<TProps>
        : ReactComponent, IComponentWithProps<TProps>
        where TProps : DefaultComponentProps
    {
        protected ReactComponent(TProps props): base(props)
        {
        }
    }

    public abstract class ReactComponent<TProps, TState>
        : ReactComponent<TProps>
        where TProps : DefaultComponentProps
        where TState : DefaultComponentState
    {
        protected TProps props;
        protected TState state;

        protected ReactComponent(TProps props): base(props)
        {
            this.props = props;
        }

        protected void CommitState()
        {

        }

    }

}