namespace Xabaril.Core
{
    public sealed class ActivatorParameterDescriptor
    {
        public string Name { get; set; }

        public string ClrType { get; set; }

        public string ActivatorName { get; set; }

        public bool IsOptional { get; set; }
    }
}
