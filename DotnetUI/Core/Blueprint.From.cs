namespace DotnetUI.Core
{
    public partial class Blueprint
    {
        // NOTE : Can I only use one generic?
        public static Blueprint From<TComponent, TComponentProps>(
            TComponentProps props
        )
            where TComponentProps : DefaultComponentProps
            where TComponent: IComponentWithProps<TComponentProps>
        {
            return new Blueprint(typeof(TComponent), props);
        }
    }
}
