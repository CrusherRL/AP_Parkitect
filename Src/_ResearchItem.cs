namespace ArchipelagoMod.Src
{
    class _ResearchItem
    {
        public int Index { get; set; }
        public string ReferenceName { get; set; }

        public _ResearchItem (int index, string referenceName)
        {
            this.ReferenceName = referenceName;
            this.Index = index;
        }
    }
}
