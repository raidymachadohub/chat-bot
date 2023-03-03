namespace Chat.Bot.Domain.DTO
{
    public class StockDTO
    {
        public string symbol { get; set; }
        public DateTime date { get; set; }
        public string time { get; set; }
        public decimal open { get; set; }
        public decimal low { get; set; }
        public decimal close { get; set; }
        public decimal volume { get; set; }

        public StockDTO()
        {
        }

        public StockDTO(string symbol, DateTime date, string time, decimal open, decimal low, decimal close,
            decimal volume)
        {
            this.symbol = symbol;
            this.date = date;
            this.time = time;
            this.open = open;
            this.low = low;
            this.close = close;
            this.volume = volume;
        }
    }
}