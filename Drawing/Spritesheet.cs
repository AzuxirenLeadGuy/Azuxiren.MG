using Microsoft.Xna.Framework.Graphics;

namespace Azuxiren.MG.Drawing;
/// <summary>Represents a spritesheet</summary>
/// <param name="sheet">The texture of the entire spritesheet</param>
public abstract class SpriteSheet(Texture2D sheet)
{
	/// <summary>The spritesheet instance</summary>
	protected readonly Texture2D _sheet = sheet;

}