namespace DotnetUI
{
    public static class React
    {
        // NOTE : Can I only use one generic?
        public static ReactElement CreateElement<TReactComponent, TReactComponentProps>(
            TReactComponentProps props
        )
            where TReactComponentProps : DefaultComponentProps
            where TReactComponent: IComponentWithProps<TReactComponentProps>
        {
            return new ReactElement(typeof(TReactComponent), props);
        }

        //public static ReactComponent CreateElement(
        //    string tag,
        //    (string key, object value)[] props
        //)
        //{
        //    return new ReactComponent(tag, props, children);
        //}
    }
}
