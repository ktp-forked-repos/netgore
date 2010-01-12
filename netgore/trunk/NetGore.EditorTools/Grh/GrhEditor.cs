using System;
using System.Drawing.Design;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using NetGore.Graphics;

namespace NetGore.EditorTools
{
    /// <summary>
    /// A <see cref="UITypeEditor"/> for selecting the <see cref="GrhData"/> to use on a <see cref="Grh"/>.
    /// </summary>
    public class GrhEditor : UITypeEditor
    {
        /// <summary>
        /// Indicates whether the specified context supports painting a representation of an object's value within th
        /// specified context.
        /// </summary>
        /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"/> that can be used to gain
        /// additional context information.</param>
        /// <returns>
        /// true if <see cref="M:System.Drawing.Design.UITypeEditor.PaintValue(System.Object,System.Drawing.Graphics,System.Drawing.Rectangle)"/>
        /// is implemented; otherwise, false.
        /// </returns>
        public override bool GetPaintValueSupported(System.ComponentModel.ITypeDescriptorContext context)
        {
            return true;
        }

        /// <summary>
        /// Paints a representation of the value of an object using the specified
        /// <see cref="T:System.Drawing.Design.PaintValueEventArgs"/>.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Drawing.Design.PaintValueEventArgs"/> that indicates what to paint and
        /// where to paint it.</param>
        public override void PaintValue(PaintValueEventArgs e)
        {
            var image = GrhImageList.TryGetImage(e.Value as Grh);
            if (image != null)
            {
                e.Graphics.DrawImage(image, e.Bounds);
            }

            base.PaintValue(e);
        }

        /// <summary>
        /// Edits the specified object's value using the editor style indicated by the
        /// <see cref="M:System.Drawing.Design.UITypeEditor.GetEditStyle"/> method.
        /// </summary>
        /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"/> that can be
        /// used to gain additional context information.</param>
        /// <param name="provider">An <see cref="T:System.IServiceProvider"/> that this editor can use to
        /// obtain services.</param>
        /// <param name="value">The object to edit.</param>
        /// <returns>
        /// The new value of the object. If the value of the object has not changed, this should return the
        /// same object it was passed.
        /// </returns>
        public override object EditValue(System.ComponentModel.ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            IWindowsFormsEditorService svc = provider.GetService(typeof(IWindowsFormsEditorService)) as IWindowsFormsEditorService;
            var grh = value as Grh;

            if (svc != null && grh != null)
            {
                using (var editorForm = new GrhUITypeEditorForm(grh))
                {
                    var originalGrhData = grh.GrhData;
                    if (svc.ShowDialog(editorForm) != DialogResult.OK)
                    {
                        // Revert to the original
                        grh.SetGrh(originalGrhData);
                    }
                }
            }

            return value;

        }

        /// <summary>
        /// Gets the editor style used by the
        /// <see cref="M:System.Drawing.Design.UITypeEditor.EditValue(System.IServiceProvider,System.Object)"/> method.
        /// </summary>
        /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"/> that can be used
        /// to gain additional context information.</param>
        /// <returns>
        /// A <see cref="T:System.Drawing.Design.UITypeEditorEditStyle"/> value that indicates the style of editor
        /// used by the <see cref="M:System.Drawing.Design.UITypeEditor.EditValue(System.IServiceProvider,System.Object)"/>
        /// method. If the <see cref="T:System.Drawing.Design.UITypeEditor"/> does not support this method,
        /// then <see cref="M:System.Drawing.Design.UITypeEditor.GetEditStyle"/> will return
        /// <see cref="F:System.Drawing.Design.UITypeEditorEditStyle.None"/>.
        /// </returns>
        public override UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }
    }
}