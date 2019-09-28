namespace DotnetUI.Core
{
    public partial class ComponentBlueprint
    {
        // NOTE : Can I only use one generic?
        public static ComponentBlueprint From<TComponent, TComponentProps>(
            TComponentProps props
        )
            where TComponentProps : IDefaultComponentProps
            where TComponent: IComponentWithProps<TComponentProps>
        {
            return new ComponentBlueprint(typeof(TComponent), props);
        }
    }
}
