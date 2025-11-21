using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace Azuxiren.MG.Animation2D;

/// <summary>
/// <para>LargeSprite consist of series of Texture2D. This is Not a SpriteSheet. </para>
/// <para>
/// If the animation consists of very large frames, this is the struct you need, provieded you are able to give
/// an array of images/Texture2D as input
/// </para>
/// </summary>
public struct LargeSprite
{
	/// <summary>
	/// The constructor for LargeSprite
	/// </summary>
	/// <param name="frames">The frames in this Sprite</param>
	/// <param name="dest">The Rectangle where all the spirtes are being displayed</param>
	/// <param name="speednum">The numenator of the ratio of speed of unrolling the sprite with respect to the framerate of the game. Don't touch if you don't understand</param>
	/// <param name="speedden">The denominator of the ratio of speed of unrolling the sprite with respect to the framerate of the game. Don't touch if you don't understand</param>
	/// <param name="next">A custom updater for frames in between. For normal propagation, set as null</param>
	public LargeSprite(IEnumerable<Texture2D> frames, Rectangle dest, byte speednum = 1, byte speedden = 1, IEnumerable<int>? next = null)
	{
		FrameImages = [.. frames];
		Dest = dest;
		Num = speednum;
		Den = speedden;
		Cur = 0;
		CurrentFrame = 0;
		if (next == null)
		{
			int len = FrameImages.Length;
			Next = [.. Enumerable.Range(1, len)];
			Next[len - 1] = 0;
		}
		else
		{
			Next = [.. next];
		}

		if (Next.Length != FrameImages.Length)
			throw new ArgumentException("Expected equal size of `frames` and `next`", nameof(next));
	}
	/// <summary>
	/// The constructor for LargeSprite
	/// </summary>
	/// <param name="frames">The frames in this Sprite</param>
	/// <param name="dest">The Rectangle where all the spirtes are being displayed</param>
	/// <param name="copy">If true, the texture and next frame array are copied, otherwise, the same reference is used</param>
	/// <param name="speednum">The numenator of the ratio of speed of unrolling the sprite with respect to the framerate of the game. Don't touch if you don't understand</param>
	/// <param name="speedden">The denominator of the ratio of speed of unrolling the sprite with respect to the framerate of the game. Don't touch if you don't understand</param>
	/// <param name="next">A custom updater for frames in between. For normal propagation, set as null</param>
	public LargeSprite(Texture2D[] frames, Rectangle dest, bool copy, byte speednum = 1, byte speedden = 1, int[]? next = null)
	{
		FrameImages = copy ? frames.ToArray() : frames;
		Dest = dest;
		Num = speednum;
		Den = speedden;
		Cur = 0;
		CurrentFrame = 0;
		if (next == null)
		{
			int len = FrameImages.Length;
			Next = Enumerable.Range(1, len).ToArray();
			Next[len - 1] = 0;
		}
		else
		{
			Next = copy ? next.ToArray() : next;
		}

		if (Next.Length != FrameImages.Length)
			throw new ArgumentException("Expected equal size of `frames` and `next`", nameof(next));
	}
	/// <summary>
	/// The sprite Image collection
	/// </summary>
	public readonly Texture2D[] FrameImages;
	/// <summary>
	/// This is where the sprite is drawn
	/// </summary>
	public Rectangle Dest;
	/// <summary>
	/// Denotes the currentFrame value.
	/// </summary>
	public int CurrentFrame;
	/// <summary>
	/// The total count of frames in the object
	/// </summary>
	public readonly int TotalFrame => FrameImages.Length;
	internal byte Num, Den, Cur;
	/// <summary>The next function for each index</summary>
	public readonly int[] Next;
	/// <summary>
	/// <para>Sets the speed of unrolling the sprite as a fraction of the current game's FPS.</para>
	/// <para>For example, if the game is 60 fps, and the function is invoked with num=1,den=2 (1/2), then the speed of unrolling this spritesheet will be (1/2) of 60fps, ie 30fps</para>
	/// </summary>
	/// <param name="num">Numenator of the fraction</param>
	/// <param name="den">Denominator of fraction</param>
	public void SetSpeed(byte num, byte den) => (Num, Den, Cur) = (num, den, 0);
	/// <summary>
	/// Updates the frame for the sprite
	/// </summary>
	public void Update()
	{
		Cur += Num;
		if (Cur >= Den)
		{
			CurrentFrame = Next[CurrentFrame];
			Cur = 0;
		}
	}
	/// <summary>
	/// Draws the Sprite using the given spritebatch
	/// </summary>
	/// <param name="sb">The given SpriteBatch</param>
	public readonly void Draw(SpriteBatch sb) => Draw(sb, Dest, Color.White, 0, Vector2.Zero);
	/// <summary>
	/// Draws the Sprite using the given spritebatch
	/// </summary>
	/// <param name="sb">The given SpriteBatch</param>
	/// <param name="dest">The temporary destination rectangle</param>
	public readonly void Draw(SpriteBatch sb, Rectangle dest) => Draw(sb, dest, Color.White, 0, Vector2.Zero);
	/// <summary>
	/// Draws the spirte using the given spritebatch
	/// </summary>
	/// <param name="sb">The given SpriteBatch</param>
	/// <param name="tint">The tint color to add</param>
	public readonly void Draw(SpriteBatch sb, Color tint) => Draw(sb, Dest, tint, 0, Vector2.Zero);
	/// <summary>
	/// Draws the sprite using the given spritebatcj
	/// </summary>
	/// <param name="spriteBatch">The given SpriteBatch</param>
	/// <param name="tint">The tint color to add</param>
	/// <param name="angle">The angle to rotate</param>
	public readonly void Draw(SpriteBatch spriteBatch, Color tint, float angle) => Draw(spriteBatch, Dest, tint, angle, FrameImages[CurrentFrame].Bounds.Center.ToVector2());
	/// <summary>
	/// Draws the sprite using the given spritebatch
	/// </summary>
	/// <param name="spriteBatch">The given SpriteBatch</param>
	/// <param name="dest">The temporary destination rectangle</param>
	/// <param name="tint">The tint color to add</param>
	/// <param name="angle">The angle to rotate</param>
	/// <param name="origin">The origin of rotation</param>
	/// <param name="effects">Added effects</param>
	/// <param name="depth">The depth in the layer for this sprite</param>
	public readonly void Draw(SpriteBatch spriteBatch, Rectangle dest, Color tint, float angle, Vector2 origin, SpriteEffects effects = SpriteEffects.None, float depth = 0) => spriteBatch.Draw(FrameImages[CurrentFrame], dest, null, tint, angle, origin, effects, depth);
}