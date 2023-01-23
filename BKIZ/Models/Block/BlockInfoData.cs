namespace BKIZ.Models.Block
{
    public class BlockInfoData
    {
        public int transaction_count { get; set; }
        public int time { get; set; }
        public string snapshot_hash { get; set; }
        public string prev_hash { get; set; }
        public int height { get; set; }
        public string hash { get; set; }
        public string datetime { get; set; }
    }
}