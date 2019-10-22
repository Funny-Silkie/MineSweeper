using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using asd;
using fslib;
using static MineSweeper.Base;

namespace MineSweeper
{
    public interface IObj
    {
        Vector2DI CellPosition { get; }
    }
    public class Character : TextObject2D, IObj
    {
        public Vector2DI CellPosition { get; }
        public byte Mines { get; private set; }
        public Character(Vector2DI cellPos)
        {
            CellPosition = cellPos;
            Position = CellPosition.To2DF() * CellSize;
            CenterPosition = new Vector2DF(1, 1) * CellSize / 2;
        }
        protected override void OnAdded()
        {
            Font = Central.Font;
            Mines = DetectMines();
            Text = Mines == 0 ? "" : Mines.ToString();
        }
        private byte DetectMines() => (byte)Layer.Objects.OfType<Mine>().Count(x => Math.Abs(x.CellPosition.X - CellPosition.X) <= 1 && Math.Abs(x.CellPosition.Y - CellPosition.Y) <= 1);
    }
    public abstract class MObject : TextureObject2D, IObj
    {
        public Vector2DI CellPosition { get; set; }
        public bool IsFragged 
        {
            get => _frag;
            set
            {
                if (value)
                    Texture = Textures["Frag"];
                else
                    Texture = Textures["Wall"];
                _frag = value;
            }
        }   
        private bool _frag = false;
        public bool MouseCollide => IsCollide();
        public bool LeftClicked => MouseCollide && Engine.Mouse.LeftButton.ButtonState == ButtonState.Push;
        public bool RightClicked => MouseCollide && Engine.Mouse.RightButton.ButtonState == ButtonState.Push;
        public MObject(Vector2DI cellPos)
        {
            CellPosition = cellPos;
            Position = CellPosition.To2DF() * CellSize;
            Texture = Textures["Wall"];
        }
        protected sealed override void OnUpdate()
        {
            if (LeftClicked)
                OnLeftClicked();
            if (RightClicked)
                OnRightClicked();
            Update();
        }
        protected virtual void Update() { }
        protected virtual void OnLeftClicked() { }
        protected virtual void OnRightClicked() 
        {
            IsFragged = !IsFragged;
        }
        private bool IsCollide()
        {
            var pos = Engine.Mouse.Position;
            var x = Position.X <= pos.X && pos.X <= Position.X + Texture.Size.X;
            var y = Position.Y <= pos.Y && pos.Y <= Position.Y + Texture.Size.Y;
            return x && y;
        }
    }
    public class Mine : MObject
    {
        public bool IsShown { get; private set; }
        public static List<Vector2DI> Positions { get; }
        static Mine()
        {
            Positions = new List<Vector2DI>();
        }
        public Mine(Vector2DI cellPos)
            : base(cellPos)
        {
            Positions.Add(cellPos);
        }
    }
    public class Wall : MObject
    {
        public Wall(Vector2DI cellPos)
            : base(cellPos) { }
        protected override void OnLeftClicked()
        {
            Layer.AddObject(new Character(CellPosition));
            Dispose();
        }
    }
}
