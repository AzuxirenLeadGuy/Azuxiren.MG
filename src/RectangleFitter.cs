using System;

using Microsoft.Xna.Framework;
namespace Azuxiren.MG
{
	public static partial class Global
	{
		/// <summary>
		/// Fits a given number of rectangles in the LargeRectangle provided as input such that all inner rectangles are of the same width and height
		/// </summary>
		/// <param name="largeRectangle">The rectangle to fit other rectangles at</param>
		/// <param name="toFit">The number of small rectangles to fit</param>
		/// <param name="offset">The cleaerance(in px) between each rectangle</param>
		/// <param name="vertical">If true, the rectangles are fitted vertically. Otherwise they are fitted horizontally</param>
		/// <returns></returns>
		public static Rectangle[] FitRectangle(this Rectangle largeRectangle, byte toFit, uint offset = 0, bool vertical = false)
		{
			if (toFit == 0) throw new ArgumentException("Invalid box count");
			else if (toFit == 1) return new Rectangle[] { largeRectangle };
			int width, height;
			if (vertical)
			{
				width = largeRectangle.Width;
				height = (int)((largeRectangle.Height - ((toFit - 1) * offset)) / toFit);
			}
			else
			{
				height = largeRectangle.Height;
				width = (int)((largeRectangle.Width - ((toFit - 1) * offset)) / toFit);
			}
			if (height <= 0 || width <= 0) throw new ArgumentException("Not possible to fit these many rectangles with the given LargeRectangle and offset");
			Rectangle[] rectangles = new Rectangle[toFit];
			int x = largeRectangle.X, y = largeRectangle.Y;
			for (byte i = 0; i < toFit; i++)
			{
				rectangles[i] = new Rectangle(x, y, width, height);
				if (vertical) y += height + (int)offset;
				else x += width + (int)offset;
			}
			return rectangles;
		}
		/// <summary>
		/// Returns a matrix of Rectangles fitted as per requirements
		/// </summary>
		/// <param name="largeRectangle">The area to divide into</param>
		/// <param name="rectsInRow">The number of rectangles desired in a single row</param>
		/// <param name="xOffset">The offset distance between each rectangle in a single row</param>
		/// <param name="rectsInCollumn">The number of rectangles in a single collumn</param>
		/// <param name="yOffset">THe offset distance between eacj rectangle in a single column</param>
		/// <returns></returns>
		public static Rectangle[,] FitRectangle(this Rectangle largeRectangle, byte rectsInRow, uint xOffset, byte rectsInCollumn, uint yOffset)
		{
			if (rectsInCollumn * rectsInRow == 0) throw new ArgumentException("Invalid box count");
			int width = (int)((largeRectangle.Height - ((rectsInRow - 1) * xOffset)) / rectsInRow), height = (int)((largeRectangle.Width - ((rectsInCollumn - 1) * yOffset)) / rectsInCollumn);
			var rects = new Rectangle[rectsInRow, rectsInCollumn];
			int x, y = largeRectangle.Y;
			for (int i = 0; i < rectsInRow; i++)
			{
				x = largeRectangle.X;
				for (int j = 0; j < rectsInCollumn; j++)
				{
					rects[i, j] = new Rectangle(x, y, width, height);
					x += width + (int)xOffset;
				}
				y += height + (int)yOffset;
			}
			return rects;
		}
		/// <summary>
		/// Fits rectangles into a single Large rectangle according to given ratios and offset
		/// </summary>
		/// <param name="largeRectangle">The large Rectangle to be divided</param>
		/// <param name="ratios">The ratio of width/height of each rectangle</param>
		/// <param name="offset">The distance between each rectangle</param>
		/// <param name="vertical">If true, the large rectangle is divided vertically, otherwise horizontally</param>
		/// <returns></returns>
		public static Rectangle[] FitRectangle(this Rectangle largeRectangle, byte[] ratios, uint offset, bool vertical = false)
		{
			var len = ratios.Length;
			if (len == 0) throw new ArgumentException("There should be at least one value in the list of ratios", nameof(ratios));
			else if (len == 1) return new Rectangle[] { largeRectangle };
			int sum = 0, i, x = largeRectangle.X, y = largeRectangle.Y, width, height;
			for (i = 0; i < len; i++) sum += ratios[i];
			if (vertical)
			{
				width = largeRectangle.Width;
				height = (int)(largeRectangle.Height - ((len - 1) * offset)) / sum;
			}
			else
			{
				height = largeRectangle.Height;
				width = (int)(largeRectangle.Width - ((len - 1) * offset)) / sum;
			}
			if (width <= 0 || height <= 0) throw new ArgumentException("Invalid dimensions for the menu", nameof(largeRectangle));
			Rectangle[] rectangles = new Rectangle[len];
			for (i = 0; i < len; i++)
			{
				if (vertical)
				{
					rectangles[i] = new Rectangle(x, y, width, height * ratios[i]);
					y += (height * ratios[i]) + (int)offset;
				}
				else
				{
					rectangles[i] = new Rectangle(x, y, width * ratios[i], height);
					x += (width * ratios[i]) + (int)offset;
				}
			}
			return rectangles;
		}
	}
}