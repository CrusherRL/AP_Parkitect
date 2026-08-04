using System.Collections.Generic;

namespace ArchipelagoMod.Src.Challenges
{
    public class ChallengeHelper
    {
        public int Count = 0;
        public int LowestCount = 0;
        public List<string> UnsolvedCheckList = new List<string>();

        public ChallengeHelper(int count)
        {
            this.Count = count;
            this.LowestCount = count;
        }

        public bool HasUnsolvedCheckList()
        {
            return this.LowestCount < this.Count;
        }

        public bool ValidCount(int count)
        {
            Helper.Debug($"[ChallengeHelper::ValidCount] this.LowestCount={this.LowestCount} count={count} this.Count={this.Count}");
            return this.LowestCount > 0 && count >= this.Count;
        }

        public void ValidateAndAddToCheckList(int count, string text)
        {
            if (this.ValidCount(count))
            {
                return;
            }

            this.LowestCount = this.LowestCount > count ? count : this.LowestCount;
            this.AddToChecklist(text);
        }

        public void AddToChecklist(string text, bool start = false)
        {
            if (start)
            {
                this.UnsolvedCheckList.Insert(0, text);
                return;
            }

            this.UnsolvedCheckList.Add(text);
        }

        public int GetCountDifference()
        {
            return this.Count - this.LowestCount;
        }

        public string Message()
        {
            return string.Join("\n", this.UnsolvedCheckList); ;
        }
    }
}
