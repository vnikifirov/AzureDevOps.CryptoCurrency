using Currency.Business.Services.Implementations;
using Currency.Business.Models;

namespace PriceMint
{
    public partial class Form1 : Form
    {
        private CurrencyService _currencyService;
        private System.Windows.Forms.Timer _timer;

        public Form1()
        {
            InitializeComponent();
            _currencyService = new CurrencyService("coincodex.com", "/api/coincodex/get_coin/");
            StartTimer();
        }

        public decimal PriceSneakers(decimal Price, decimal PriceSneakers) =>  Price * PriceSneakers;
        public decimal PriceLvlUp(decimal PriceGst,decimal PriceGmt, decimal PriceLvlUpGst, decimal PriceLvlUpGmt) => (PriceGst * PriceLvlUpGst)+(PriceGmt * PriceLvlUpGmt);
        public decimal PriceMint(decimal PriceGst, decimal PriceGmt, decimal PriceMintGst, decimal PriceMintGmt) => (PriceGst * PriceMintGst)+(PriceGmt * PriceMintGmt);
        public decimal PriceMintPlusLvlUp(decimal PriceMint, decimal PriceLvlUp) => (PriceMint + PriceLvlUp);
        public decimal Profit(decimal MinPriceSneakers, decimal _priceMintPlusLvlUp) => (MinPriceSneakers - _priceMintPlusLvlUp);


        public void ManualMintPriceGstAndGmt()
        {
            decimal.TryParse(gst.Text, out decimal _gst);
            if (_gst < 2)
            {
                PriceMintGst.Text = "200";
                PriceMintGmt.Text = "0";
            }
            else if(_gst>=2 & _gst < 3)
            {
                PriceMintGst.Text = "160";
                PriceMintGmt.Text = "40";
            }
            else if (_gst >=3 & _gst < 4)
            {
                PriceMintGst.Text = "120";
                PriceMintGmt.Text = "80";
            }
            else if (_gst >=4 & _gst < 8)
            {
                PriceMintGst.Text = "100";
                PriceMintGmt.Text = "100";
            }
            else if (_gst >=8 & _gst < 10)
            {
                PriceMintGst.Text = "80";
                PriceMintGmt.Text = "120";
            }
            else
            {
                PriceMintGst.Text = "40";
                PriceMintGmt.Text = "160";
            }
        }

        public void ProFIt()
        {
            decimal.TryParse(PriceSneakersDollarFive.Text, out decimal _priceSneakersDollarFive);
            decimal.TryParse(PriceSneakersMintPlusLvlUp.Text, out decimal _priceSneakersMintPlusLvlUp);
            ProfitLable.Text = Profit(_priceSneakersDollarFive, _priceSneakersMintPlusLvlUp).ToString();
        }
        public void PriceMintPlusLvlUp()
        {
            decimal.TryParse(PriceMintDol.Text, out decimal _priceMintDol);
            decimal.TryParse(PriceLvlUpDol.Text, out decimal _priceLvlUpDol);
            PriceSneakersMintPlusLvlUp.Text = PriceMintPlusLvlUp(_priceMintDol,_priceLvlUpDol).ToString();
        }
        public void MinPriceSneakers()
        {
            decimal.TryParse(Solana.Text, out decimal solanaPrice);
            decimal.TryParse(PriceSneakersMinSolZero.Text, out decimal PriceSneakersZeroMinSolPrice);
            PriceSneakersDollarZero.Text = PriceSneakers(solanaPrice, PriceSneakersZeroMinSolPrice).ToString();
            decimal.TryParse(PriceSneakersMinSolFive.Text, out decimal PriceSneakersFiveMinSolPrice);
            PriceSneakersDollarFive.Text = PriceSneakers(solanaPrice, PriceSneakersFiveMinSolPrice).ToString();
        }

        public void PriceLvlUpZeroToFive()
        {
            decimal.TryParse(gst.Text, out decimal gstPrice);
            decimal.TryParse(ManualPriceLvlUpGst.Text, out decimal ManualPriceLvlUpGstText);
            decimal.TryParse(gmt.Text, out decimal gmtPrice);
            decimal.TryParse(ManualPriceLvlUpGmt.Text, out decimal ManualPriceLvlUpGmtText);
            PriceLvlUpDol.Text = PriceLvlUp(gstPrice,gmtPrice,ManualPriceLvlUpGstText,ManualPriceLvlUpGmtText).ToString();
        }

        public void PriceMint()
        {
            decimal.TryParse(gst.Text, out decimal gstPrice);
            decimal.TryParse(PriceMintGst.Text, out decimal _priceMintGst);
            decimal.TryParse(gmt.Text, out decimal gmtPrice);
            decimal.TryParse(PriceMintGmt.Text, out decimal _priceMintGmt);
            PriceMintDol.Text = PriceLvlUp(gstPrice, gmtPrice, _priceMintGst, _priceMintGmt).ToString();
        }

        private async Task DoWork()
        {

            Action action = async () =>
            {
                gst.Text = (await _currencyService.GetCurrencyAsync<Gst>("gst2")).ToString();
                gmt.Text = (await _currencyService.GetCurrencyAsync<Gmt>("gmt")).ToString();
                Solana.Text = (await _currencyService.GetCurrencyAsync<Solana>("Sol")).ToString();
                MinPriceSneakers();
                ManualMintPriceGstAndGmt();
                PriceLvlUpZeroToFive();
                PriceMint();
                PriceMintPlusLvlUp();
                ProFIt();
                // other currency 
            };

            Invoke(action);

        }

        public void StartTimer()
        {
            // _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));
            _timer = new System.Windows.Forms.Timer();
            _timer.Interval = (int)TimeSpan.FromSeconds(5).TotalMilliseconds;
            _timer.Tick += async (sender, e) => await DoWork();
            _timer.Start();
        }
    }
}