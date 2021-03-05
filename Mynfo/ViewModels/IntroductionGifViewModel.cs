namespace Mynfo.ViewModels
{
    public class IntroductionGifViewModel : BaseViewModel
    {

        private string gitImage;
        public string GitImage
        {
            get { return this.gitImage; }
            set { SetValue(ref this.gitImage, value); }
        }
        public IntroductionGifViewModel()
        {
            GitImage = "GIF_mynfo_general.gif";
        }
    }
}
