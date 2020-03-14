using System;
using Microsoft.Xna.Framework;
namespace Azuxiren.MG
{
	/// <summary>
	/// The various modes to use FitRectangle methods
	/// </summary>
	public enum FitRectanglesMode:byte
	{
		/// <summary>Vertical stacks</summary>
		StackVertically,
		/// <summary>Horizontal stacks</summary>
		StackHorizontally
	}
	public static partial class Global
	{
		/// <summary>
		/// Fits a given number of rectangles in the LargeRectangle provided as input such that all inner rectangles are of the same width and height
		/// </summary>
		/// <param name="LargeRectangle">The rectangle to fit other rectangles at</param>
		/// <param name="ToFit">The number of small rectangles to fit</param>
		/// <param name="offset">The cleaerance(in px) between each rectangle</param>
		/// <param name="mode">Allows stacking of rectangles horizontally or verically</param>
		/// <returns></returns>
		public static Rectangle[] FitRectangles(Rectangle LargeRectangle, byte ToFit, int offset=0, FitRectanglesMode mode=FitRectanglesMode.StackVertically)
		{
			if(ToFit==0||offset<0)throw new ArgumentException("Invalid Offset or box count");
			else if(ToFit==1)return new Rectangle[]{LargeRectangle};
			bool Vertical=mode==FitRectanglesMode.StackVertically;
			int width,height;
			if(Vertical)
			{
				width=LargeRectangle.Width;
				height=(LargeRectangle.Height-((ToFit-1)*offset))/(ToFit+1);
			}
			else
			{
				height=LargeRectangle.Height;
				width=(LargeRectangle.Width-((ToFit-1)*offset))/(ToFit+1);
			}
			if(height<=0||width<=0)throw new ArgumentException("Not possible to fit these many rectangles with the given LargeRectangle and offset");
			Rectangle[] rectangles=new Rectangle[ToFit];
			int x=LargeRectangle.X,y=LargeRectangle.Y;
			for(byte i=0;i<ToFit;i++)
			{
				rectangles[i]=new Rectangle(x,y,width,height);
				if(Vertical)y+=height+offset;
				else x+=width+offset;
			}
			return rectangles;
		}
	}
}