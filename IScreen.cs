using Microsoft.Xna.Framework;
namespace Azuxiren.MG
{
	/// <summary>
	/// The Common Interface to use for all Screens
	/// </summary>
	public interface IScreen
	{
		/// <summary>
		/// Loads all the components of the screen
		/// </summary>
		void LoadContent();
		/// <summary>
		/// Updates a frame of the screen
		/// </summary>
		/// <param name="gt">Denotes an instant of time in Monogame</param>
		void Update(GameTime gt);
		/// <summary>
		/// Draws a frame of a game
		/// </summary>
		/// <param name="gt">Denotes an instant of time in Monogame</param>
		void Draw(GameTime gt);
	}
}