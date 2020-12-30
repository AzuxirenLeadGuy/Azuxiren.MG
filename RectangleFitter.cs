using System;
using Microsoft.Xna.Framework;
namespace Azuxiren.MG
{
	public static partial class Global
	{
		/// <summary>
		/// Fits a given number of rectangles in the LargeRectangle provided as input such that all inner rectangles are of the same width and height
		/// </summary>
		/// <param name="LargeRectangle">The rectangle to fit other rectangles at</param>
		/// <param name="ToFit">The number of small rectangles to fit</param>
		/// <param name="offset">The cleaerance(in px) between each rectangle</param>
		/// <param name="Vertical">If true, the rectangles are fitted vertically. Otherwise they are fitted horizontally</param>
		/// <returns></returns>
		public static Rectangle[] FitRectangle(this Rectangle LargeRectangle, byte ToFit, uint offset=0, bool Vertical=false)
		{
			if(ToFit==0)throw new ArgumentException("Invalid box count");
			else if(ToFit==1)return new Rectangle[]{LargeRectangle};
			int width,height;
			if(Vertical)
			{
				width=LargeRectangle.Width;
				height=(int)((LargeRectangle.Height-((ToFit-1)*offset))/ToFit);
			}
			else
			{
				height=LargeRectangle.Height;
				width=(int)((LargeRectangle.Width-((ToFit-1)*offset))/ToFit);
			}
			if(height<=0||width<=0)throw new ArgumentException("Not possible to fit these many rectangles with the given LargeRectangle and offset");
			Rectangle[] rectangles=new Rectangle[ToFit];
			int x=LargeRectangle.X,y=LargeRectangle.Y;
			for(byte i=0;i<ToFit;i++)
			{
				rectangles[i]=new Rectangle(x,y,width,height);
				if(Vertical)y+=height+(int)offset;
				else x+=width+(int)offset;
			}
			return rectangles;
		}
		/// <summary>
		/// Returns a matrix of Rectangles fitted as per requirements
		/// </summary>
		/// <param name="LargeRectangle">The area to divide into</param>
		/// <param name="RectsInRow">The number of rectangles desired in a single row</param>
		/// <param name="xOffset">The offset distance between each rectangle in a single row</param>
		/// <param name="RectsInCollumn">The number of rectangles in a single collumn</param>
		/// <param name="yOffset">THe offset distance between eacj rectangle in a single column</param>
		/// <returns></returns>
		public static Rectangle[,] FitRectangle(this Rectangle LargeRectangle, byte RectsInRow, uint xOffset, byte RectsInCollumn, uint yOffset)
		{
			if(RectsInCollumn*RectsInRow==0)throw new ArgumentException("Invalid box count");
			int width=(int)((LargeRectangle.Height-((RectsInRow-1)*xOffset))/RectsInRow),height=(int)((LargeRectangle.Width-((RectsInCollumn-1)*yOffset))/RectsInCollumn);
			var Rects=new Rectangle[RectsInRow,RectsInCollumn];
			int x,y=LargeRectangle.Y;
			for(int i=0;i<RectsInRow;i++)
			{
				x=LargeRectangle.X;
				for(int j=0;j<RectsInCollumn;j++)
				{
					Rects[i,j]=new Rectangle(x,y,width,height);
					x+=width+(int)xOffset;
				}
				y+=height+(int)yOffset;
			}
			return Rects;
		}
		/// <summary>
		/// Fits rectangles into a single Large rectangle according to given ratios and offset
		/// </summary>
		/// <param name="LargeRectangle">The large Rectangle to be divided</param>
		/// <param name="Ratios">The ratio of width/height of each rectangle</param>
		/// <param name="offset">The distance between each rectangle</param>
		/// <param name="Vertical">If true, the large rectangle is divided vertically, otherwise horizontally</param>
		/// <returns></returns>
		public static Rectangle[] FitRectangle(this Rectangle LargeRectangle, byte[] Ratios, uint offset, bool Vertical=false)
		{
			var len=Ratios.Length;
			if(len==0)throw new ArgumentException();
			else if(len==1)return new Rectangle[]{LargeRectangle};
			int sum=0,i,x=LargeRectangle.X,y=LargeRectangle.Y,width,height;
			for(i=0;i<len;i++)sum+=Ratios[i];
			if(Vertical)
			{
				width=LargeRectangle.Width;
				height=(int)(LargeRectangle.Height-((len-1)*offset))/sum;
			}
			else
			{
				height=LargeRectangle.Height;
				width=(int)(LargeRectangle.Width-((len-1)*offset))/sum;
			}
			if(width<=0||height<=0)throw new ArgumentException();
			Rectangle[] rectangles=new Rectangle[len];
			for(i=0;i<len;i++)
			{
				if(Vertical)
				{
					rectangles[i]=new Rectangle(x,y,width,height*Ratios[i]);
					y+=(height*Ratios[i])+(int)offset;
				}
				else
				{
					rectangles[i]=new Rectangle(x,y,width*Ratios[i],height);
					x+=(width*Ratios[i])+(int)offset;
				}
			}
			return rectangles;
		}
	}
}