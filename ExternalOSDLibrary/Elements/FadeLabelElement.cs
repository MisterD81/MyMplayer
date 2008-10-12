#region Copyright (C) 2006-2008 MisterD

/* 
 *	Copyright (C) 2006-2008 MisterD
 *
 *  This Program is free software; you can redistribute it and/or modify
 *  it under the terms of the GNU General Public License as published by
 *  the Free Software Foundation; either version 2, or (at your option)
 *  any later version.
 *   
 *  This Program is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
 *  GNU General Public License for more details.
 *   
 *  You should have received a copy of the GNU General Public License
 *  along with GNU Make; see the file COPYING.  If not, write to
 *  the Free Software Foundation, 675 Mass Ave, Cambridge, MA 02139, USA. 
 *  http://www.gnu.org/copyleft/gpl.html
 *
 */

#endregion

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using MediaPortal.GUI.Library;

namespace ExternalOSDLibrary
{
  /// <summary>
  /// This class represents a GUIFadeLabel
  /// </summary>
  public class FadeLabelElement : BaseElement
  {
    #region variables
    /// <summary>
    /// GUIFadeLabel
    /// </summary>
    private GUIFadeLabel _label;

    /// <summary>
    /// Font
    /// </summary>
    private Font _font;

    /// <summary>
    /// Brush
    /// </summary>
    private Brush _brush;

    /// <summary>
    /// Label of the fade label
    /// </summary>
    private String _labelString;
    #endregion

    #region ctor
    /// <summary>
    /// Creates the element and retrieves all information from the control
    /// </summary>
    /// <param name="control">GUIControl</param>
    public FadeLabelElement(GUIControl control)
      : base(control)
    {
      _label = control as GUIFadeLabel;
      _font = getFont(_label.FontName);
      _brush = new SolidBrush(GetColor(_label.TextColor));
      _labelString = _label.Label;
      Log.Debug("VideoPlayerOSD: Found label element: " + _label.Name + "/" + _font.Name);
    }
    #endregion

    #region implmenented abstract method
    /// <summary>
    /// Draws the element on the given graphics
    /// </summary>
    /// <param name="graph">Graphics</param>
    public override void DrawElement(Graphics graph)
    {
      if (_label.Visible)
      {
        GUIControl.Alignment alignment = _label.TextAlignment;
        RectangleF rectangle;
        String text = GUIPropertyManager.Parse(_label.Label);
        SizeF sizeF = graph.MeasureString(text, _font);
        if (alignment == GUIControl.Alignment.ALIGN_LEFT)
        {
          rectangle = new RectangleF((float)_label.XPosition, (float)_label.YPosition, _label._width, Math.Max(sizeF.Height, _label._height));
        }
        else if (alignment == GUIControl.Alignment.ALIGN_RIGHT)
        {
          rectangle = new RectangleF((float)_label.Location.X - sizeF.Width, (float)_label.Location.Y, _label.Width, Math.Max(sizeF.Height, _label.Height));
        }
        else
        {
          rectangle = new RectangleF((float)_label.Location.X - (sizeF.Width / 2), (float)_label.Location.Y - (sizeF.Height / 2), _label.Width, Math.Max(sizeF.Height, _label.Height));
        }
        graph.DrawString(text, _font, _brush, rectangle, StringFormat.GenericTypographic);
      }
    }

    /// <summary>
    /// Disposes the object
    /// </summary>
    public override void Dispose()
    {
      _font.Dispose();
      _brush.Dispose();
    }

    /// <summary>
    /// Checks, if an update for the element is needed
    /// </summary>
    /// <returns>true, if an update is needed</returns>
    protected override bool CheckElementSpecificForUpdate()
    {
      bool result = false;
      String newLabel = GUIPropertyManager.Parse(_label.Label);
      if (!newLabel.Equals(_labelString))
      {
        _labelString = newLabel;
        result = true;
      }
      return result;
    }
    #endregion
  }
}