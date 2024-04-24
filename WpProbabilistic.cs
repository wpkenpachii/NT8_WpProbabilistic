#region Using declarations
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Xml.Serialization;
using NinjaTrader.Cbi;
using NinjaTrader.Gui;
using NinjaTrader.Gui.Chart;
using NinjaTrader.Gui.SuperDom;
using NinjaTrader.Gui.Tools;
using NinjaTrader.Data;
using NinjaTrader.NinjaScript;
using NinjaTrader.Core.FloatingPoint;
using NinjaTrader.NinjaScript.DrawingTools;
#endregion

//This namespace holds Indicators in this folder and is required. Do not change it. 
namespace NinjaTrader.NinjaScript.Indicators.WPKenpachi
{
	public class WpProbabilistic : Indicator
	{
		private double NumOfBullishCandles	= 0.0;
		private double NumOfBearishCandles	= 0.0;
		
		private double PercentageOfBullishCandles;
		private double PercentageOfBearishCandles;
		private double PercentageOfUntie			= 16.0;
		
		protected override void OnStateChange()
		{
			if (State == State.SetDefaults)
			{
				Description									= @"Enter the description for your new custom Indicator here.";
				Name										= "WpProbabilistic";
				Calculate									= Calculate.OnBarClose;
				IsOverlay									= false;
				DisplayInDataBox							= true;
				DrawOnPricePanel							= true;
				DrawHorizontalGridLines						= true;
				DrawVerticalGridLines						= true;
				PaintPriceMarkers							= true;
				ScaleJustification							= NinjaTrader.Gui.Chart.ScaleJustification.Right;
				//Disable this property if your indicator requires custom values that cumulate with each new market data event. 
				//See Help Guide for additional information.
				IsSuspendedWhileInactive					= true;
				NumOfBarsAgo					= 144;
				UntiePercentage					= 16;
				EnableLogs						= false;
				AddPlot(Brushes.White, "CurrentUntiePercentage");
				AddPlot(Brushes.DarkRed, "TopUntieLine");
				AddPlot(Brushes.DarkGreen, "BottomUntieLine");
			}
			else if (State == State.Configure)
			{
				
			}
			else if (State == State.DataLoaded) {
				CurrentUntiePercentage[0] = CalculateUntiePercentage();
				PlotBrushes[0][0] = Brushes.White;
				
				TopUntieLine[0] = UntiePercentage;
				PlotBrushes[1][0] = Brushes.DarkRed;
				
				BottomUntieLine[0] = -UntiePercentage;
				PlotBrushes[2][0] = Brushes.DarkGreen;
			}
		}

		protected override void OnBarUpdate()
		{
			//Add your custom indicator logic here.
			if (BarsInProgress != 0) 
				return;
			
			if (CurrentBars[0] < NumOfBarsAgo)
				return;
			
			CountBullishAndBearishCandles();
			CalculateCandlePercentages();
			
			CurrentUntiePercentage[0] = CalculateUntiePercentage();
			TopUntieLine[0] = UntiePercentage;
			BottomUntieLine[0] = -UntiePercentage;
		}
		
		private void CountBullishAndBearishCandles() {
			NumOfBullishCandles = 0;
			NumOfBearishCandles = 0;
			for(int i = 0; i < NumOfBarsAgo; i++) {
				if (IsBullishCandle(Open[i], Close[i])) {
					NumOfBullishCandles += 1;
				} else if (IsBearishCandle(Open[i], Close[i])) {
					NumOfBearishCandles += 1;
				}
			}
		}
		
		private void CalculateCandlePercentages() {
			PercentageOfBullishCandles = (NumOfBullishCandles / NumOfBarsAgo) * 100;
			PercentageOfBearishCandles = (NumOfBearishCandles / NumOfBarsAgo) * 100;
			if (EnableLogs) {
				Print("NumOfBullishCandles: " + NumOfBullishCandles);
				Print("NumOfBearishCandles: " + NumOfBearishCandles);
				Print($"PercentageOfBullishCandles: {PercentageOfBullishCandles}");
				Print($"PercentageOfBearishCandles: {PercentageOfBearishCandles}");
			}
		}
		
		private double CalculateUntiePercentage() {
			double res = PercentageOfBullishCandles - PercentageOfBearishCandles;
			if (EnableLogs) {
				Print("UntiePercentage: " + res);
			}
			return res;
		}
		
		private bool IsBullishCandle(double _Open, double _Close) {
			return _Open < _Close;
		}
		private bool IsBearishCandle(double _Open, double _Close) {
			return _Open >= _Close;
		}
		
		#region Properties
		[NinjaScriptProperty]
		[Range(1, int.MaxValue)]
		[Display(Name="NumOfBarsAgo", Order=1, GroupName="Settings")]
		public int NumOfBarsAgo
		{ get; set; }

		[NinjaScriptProperty]
		[Range(1, 100)]
		[Display(Name="UntiePercentage", Order=2, GroupName="Settings")]
		public int UntiePercentage
		{ get; set; }
		
		[NinjaScriptProperty]
		[Display(Name="EnableLogs", Order=2, GroupName="Settings")]
		public bool EnableLogs
		{ get; set; }
		
		[Browsable(false)]
		[XmlIgnore]
		public Series<double> CurrentUntiePercentage
		{
			get { return Values[0]; }
		}
		
		[Browsable(false)]
		[XmlIgnore]
		public Series<double> TopUntieLine
		{
			get { return Values[1]; }
		}

		[Browsable(false)]
		[XmlIgnore]
		public Series<double> BottomUntieLine
		{
			get { return Values[2]; }
		}
		#endregion

	}
}

#region NinjaScript generated code. Neither change nor remove.

namespace NinjaTrader.NinjaScript.Indicators
{
	public partial class Indicator : NinjaTrader.Gui.NinjaScript.IndicatorRenderBase
	{
		private WPKenpachi.WpProbabilistic[] cacheWpProbabilistic;
		public WPKenpachi.WpProbabilistic WpProbabilistic(int numOfBarsAgo, int untiePercentage, bool enableLogs)
		{
			return WpProbabilistic(Input, numOfBarsAgo, untiePercentage, enableLogs);
		}

		public WPKenpachi.WpProbabilistic WpProbabilistic(ISeries<double> input, int numOfBarsAgo, int untiePercentage, bool enableLogs)
		{
			if (cacheWpProbabilistic != null)
				for (int idx = 0; idx < cacheWpProbabilistic.Length; idx++)
					if (cacheWpProbabilistic[idx] != null && cacheWpProbabilistic[idx].NumOfBarsAgo == numOfBarsAgo && cacheWpProbabilistic[idx].UntiePercentage == untiePercentage && cacheWpProbabilistic[idx].EnableLogs == enableLogs && cacheWpProbabilistic[idx].EqualsInput(input))
						return cacheWpProbabilistic[idx];
			return CacheIndicator<WPKenpachi.WpProbabilistic>(new WPKenpachi.WpProbabilistic(){ NumOfBarsAgo = numOfBarsAgo, UntiePercentage = untiePercentage, EnableLogs = enableLogs }, input, ref cacheWpProbabilistic);
		}
	}
}

namespace NinjaTrader.NinjaScript.MarketAnalyzerColumns
{
	public partial class MarketAnalyzerColumn : MarketAnalyzerColumnBase
	{
		public Indicators.WPKenpachi.WpProbabilistic WpProbabilistic(int numOfBarsAgo, int untiePercentage, bool enableLogs)
		{
			return indicator.WpProbabilistic(Input, numOfBarsAgo, untiePercentage, enableLogs);
		}

		public Indicators.WPKenpachi.WpProbabilistic WpProbabilistic(ISeries<double> input , int numOfBarsAgo, int untiePercentage, bool enableLogs)
		{
			return indicator.WpProbabilistic(input, numOfBarsAgo, untiePercentage, enableLogs);
		}
	}
}

namespace NinjaTrader.NinjaScript.Strategies
{
	public partial class Strategy : NinjaTrader.Gui.NinjaScript.StrategyRenderBase
	{
		public Indicators.WPKenpachi.WpProbabilistic WpProbabilistic(int numOfBarsAgo, int untiePercentage, bool enableLogs)
		{
			return indicator.WpProbabilistic(Input, numOfBarsAgo, untiePercentage, enableLogs);
		}

		public Indicators.WPKenpachi.WpProbabilistic WpProbabilistic(ISeries<double> input , int numOfBarsAgo, int untiePercentage, bool enableLogs)
		{
			return indicator.WpProbabilistic(input, numOfBarsAgo, untiePercentage, enableLogs);
		}
	}
}

#endregion
