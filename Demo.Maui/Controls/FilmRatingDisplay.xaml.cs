﻿namespace Demo.Maui.Controls;

public partial class FilmRatingDisplay : ContentView
{
    #region Fields and Properties

    public static readonly BindableProperty AverageRatingProperty =
            BindableProperty.CreateAttached(nameof(AverageRating),
                                            returnType: typeof(decimal),
                                            declaringType: typeof(FilmRatingDisplay),
                                            defaultValue: default(decimal),
                                            propertyChanged: OnAverageRatingPropertyChanged);

    public static readonly BindableProperty RatedCountProperty =
        BindableProperty.CreateAttached(nameof(RatedCount),
                                        returnType: typeof(decimal),
                                        declaringType: typeof(FilmRatingDisplay),
                                        defaultValue: default(decimal),
                                        propertyChanged: OnRatedCountPropertyChanged);

    public decimal AverageRating
    {
        get => (decimal)GetValue(AverageRatingProperty);
        set => SetValue(AverageRatingProperty, value);
    }

    public decimal RatedCount
    {
        get => (decimal)GetValue(RatedCountProperty);
        set => SetValue(RatedCountProperty, value);
    }

    #endregion

    #region Constructor

    public FilmRatingDisplay()
	{
		InitializeComponent();
	}

    #endregion

    #region Helper Methods

    private static void OnRatedCountPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var view = bindable as FilmRatingDisplay;

        view.RenderCountOfRatings();
    }

    private static void OnAverageRatingPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var view = bindable as FilmRatingDisplay;

        int fiveScaleRating = Convert.ToInt32(view.AverageRating / 2);

        view.RenderFilmRatingDisplayOf(fiveScaleRating);
    }

    private void RenderFilmRatingDisplayOf(int fiveScaleRating)
    {
        this.FilmRatingDisplayGrid.Children.Clear();
        this.FilmRatingDisplayGrid.ColumnDefinitions.Clear();

        for (int column = 0; column < 5; column++)
        {
            this.FilmRatingDisplayGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = 30 });

            Image star = new Image();

            star.Source = column < fiveScaleRating ? "star_solid.png" : "star_empty.png";

            this.FilmRatingDisplayGrid.Add(star, column, 0);
        }
    }

    private void RenderCountOfRatings()
    {
        this.RatedCountLabel.Text = this.RatedCount.ToString();
    }

    #endregion
}
