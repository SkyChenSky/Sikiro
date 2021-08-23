using System.ComponentModel.DataAnnotations;

namespace Sikiro.ES.Api.Attribute
{
    public class TableColsAttribute : System.Attribute
    {
        public TableColsAttribute()
        {
            Align = EAlign.Left;
        }
        public string Field { get; set; }

        public string Tile { get; set; }

        public int Width { get; set; }

        public EAlign Align { get; set; }

        private bool _isImage = false;
        public bool IsImage { get => _isImage; set => _isImage = value; }
    }

    public enum EAlign
    {
        [Display(Name = "center")]
        Center = 0,
        [Display(Name = "right")]
        Right = 1,
        [Display(Name = "left")]
        Left = 2,
    }
}
