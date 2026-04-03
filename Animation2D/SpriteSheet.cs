using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace Azuxiren.MG.Animation2D;
///<summary><para>SpriteSheet Manager. </para>
/// <para>Only use Draw and Update methods once initialized</para></summary>
public struct SpriteSheet
{
	///<summary>SpriteSheet Image</summary>
	internal Texture2D Sheet;

	///<summary>Source Rectangle Maps the image on the source Spritesheet</summary>
	internal Rectangle Source;

	///<summary>Keeps track of Frames on the SpriteSheet</summary>
	internal int[] FrameX, FrameY, Next;

	///<summary>The Count of Frames</summary>
	public readonly int Frames;

	///<summary>The Current Frame</summary>
	private int _currentFrame;

	///<summary>The Constructor that *MUST* be used</summary>
	///<param name="sh">Sheet Image Texture</param>
	///<param name="fw">Frame-width</param>
	///<param name="fh">Frame-height</param>
	///<param name="lx">Last Frame-X count (Starts from 0)</param>
	///<param name="ly">Last Frame-Y count (Starts from 0)</param>
	public SpriteSheet(Texture2D sh, int fw, int fh, int lx, int ly)
	{
		Sheet = sh;
		int framesPerLine = sh.Width / fw;
		Source = new Rectangle(0, 0, fw, fh);
		Frames = (framesPerLine * ly) + lx + 1;
		FrameX = new int[Frames];
		FrameY = new int[Frames];
		Next = new int[Frames];
		_currentFrame = 0;
		int i, j;
		for (i = 0; i < ly; i++)
		{
			for (j = 0; j < framesPerLine; j++)
			{
				FrameX[_currentFrame] = j * fw;
				FrameY[_currentFrame] = i * fh;
				Next[_currentFrame] = _currentFrame + 1;
				_currentFrame++;
			}
		}
		for (j = 0; j <= lx; j++)
		{
			FrameX[_currentFrame] = j * fw;
			FrameY[_currentFrame] = ly * fh;
			Next[_currentFrame] = _currentFrame + 1;
			_currentFrame++;
		}
		Next[Frames - 1] = 0;
		_currentFrame = 0;
	}

	/// <summary>
	/// Copy the properties of another spritesheet, sharing the same reference of Texture2D Spritesheet image
	/// </summary>
	/// <param name="source"></param>
	public SpriteSheet(SpriteSheet source)
	{
		Sheet = source.Sheet;
		Next = source.Next;
		FrameX = source.FrameX;
		FrameY = source.FrameY;
		Frames = source.Frames;
		Source = source.Source;
		_currentFrame = 0;
	}

	///<summary>Sets the Current Animation frame at f</summary>
	/// <param name="f">The frame value to set</param>
	public void SetFrame(int f) => _currentFrame = f;

	/// <summary>
	/// Draws the SpriteSheet
	/// </summary>
	/// <param name="sb">SpriteBatch object to use</param>
	/// <param name="dest">The Rectangle to draw the sheet frame at. (You are better off using the Dest variable instide the Spritesheet class</param>
	/// <param name="tint">The Color to tint the drawing with</param>
	public readonly void Draw(SpriteBatch sb, Rectangle dest, Color? tint) => sb.Draw(Sheet, dest, Source, tint ?? Color.White);

	///<summary><para>The Update Function of SpriteSheet</para>
	/// <para>Not Calling Update "Pauses" the Animation.</para></summary>
	public void Update()
	{
		Source.X = FrameX[_currentFrame];
		Source.Y = FrameY[_currentFrame];
		_currentFrame = Next[_currentFrame];
	}
}