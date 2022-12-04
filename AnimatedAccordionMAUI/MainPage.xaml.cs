namespace AnimatedAccordionMAUI;

public partial class MainPage : ContentPage
{
	public MainPage()
	{
		InitializeComponent();
	}
    private bool _collapsed = false;
	private double _height;
	private double _width;

#if !IOS
	protected override void OnSizeAllocated(double width, double height)
    {
        base.OnSizeAllocated(width, height);

        _height = items.Height;
        _width = items.Width;
    }
#endif

    void TapGestureRecognizer_Tapped(System.Object sender, System.EventArgs e)
    {
		if(DeviceInfo.Current.Platform == DevicePlatform.iOS && _height <= 0)
		{
            _height = items.Height;
            _width = items.Width;
        }

        var animation = new Animation(delta =>
		{
			if (!_collapsed) //Collapsing
			{
				// lets assume default height => 100
				// 0, 0.1, 0.25, 0.4, 0.6, 0.85, 0.9, 1
				// 0.1 -> 100 * 0.1 => 10, 0.25 => 100*0.25 => 25, 0.4 => 100 * 0.4 => 40, .... 100 * 1 => 100
				// 100 - 10 => 90,	100 - 25 => 75,		100 - 40 => 60, ...... 100 - 100 => 0
				var newHeight = _height - (_height * delta);
				var newWidth = _width - (_width * delta);

				(items.HeightRequest, items.WidthRequest) = (newHeight, newWidth);
			}
			else //Expanding
			{
                var newHeight = (_height * delta);
                var newWidth = (_width * delta);

                (items.HeightRequest, items.WidthRequest) = (newHeight, newWidth);
            }
        });

        icon.RotateTo(_collapsed ? 90 : -90);

        animation.Commit(this, "accordion",
			finished: (_, __) => _collapsed = !_collapsed);
    }
}

