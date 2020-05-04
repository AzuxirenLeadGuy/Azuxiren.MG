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
		public static Rectangle[] FitRectangles(Rectangle LargeRectangle, byte ToFit, uint offset=0, bool Vertical=true)
		{
			if(ToFit==0)throw new ArgumentException("Invalid box count");
			else if(ToFit==1)return new Rectangle[]{LargeRectangle};
			int width,height;
			if(Vertical)
			{
				width=LargeRectangle.Width;
				height=(int)((LargeRectangle.Height-((ToFit-1)*offset))/(ToFit+1));
			}
			else
			{
				height=LargeRectangle.Height;
				width=(int)((LargeRectangle.Width-((ToFit-1)*offset))/(ToFit+1));
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
		public static Rectangle[,] FitRectangle(Rectangle LargeRectangle, byte RectsInRow, uint xOffset, byte RectsInCollumn, uint yOffset)
		{
			if(RectsInCollumn*RectsInRow==0)throw new ArgumentException("Invalid box count");
			int width=(int)((LargeRectangle.Height-((RectsInRow-1)*xOffset))/(RectsInRow+1)),height=(int)((LargeRectangle.Width-((RectsInCollumn-1)*yOffset))/(RectsInCollumn+1));
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
	}
}