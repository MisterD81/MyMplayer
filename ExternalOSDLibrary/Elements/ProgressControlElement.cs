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
using System.IO;
using System.Text;
using MediaPortal.GUI.Library;

namespace ExternalOSDLibrary
{
  /// <summary>
  /// This class represents a GUIProgressControl
  /// </summary>
  public class ProgressControlElement : BaseElement
  {
    #region variables
    /// <summary>
    /// GUIProgressControl
    /// </summary>
    private GUIProgressControl _progressControl;

    /// <summary>
    /// Left image
    /// </summary>
    private Bitmap _leftBitmap;

    /// <summary>
    /// Middle image
    /// </summary>
    private Bitmap _midBitmap;

    /// <summary>
    /// Right image
    /// </summary>
    private Bitmap _rightBitmap;

    /// <summary>
    /// Background image
    /// </summary>
    private Bitmap _backgroundBitmap;

    /// <summary>
    /// Percentage of the progress control
    /// </summary>
    private int _percentage;
    #endregion

    #region ctor
    /// <summary>
    /// Creates the element and retrieves all information from the control
    /// </summary>
    /// <param name="control">GUIControl</param>
    public ProgressControlElement(GUIControl control)
      : base(control)
    {
      _progressControl = control as GUIProgressControl;
      _leftBitmap = loadBitmap(_progressControl.BackTextureLeftName);
      _midBitmap = loadBitmap(_progressControl.BackTextureMidName);
      _rightBitmap = loadBitmap(_progressControl.BackTextureRightName);
      _backgroundBitmap = loadBitmap(_progressControl.BackGroundTextureName);
      _percentage = GetPercentage();
      Log.Debug("VideoPlayerOSD: Found progess control");
    }
    #endregion

    #region implmenented abstract method
    /// <summary>
    /// Draws the element on the given graphics
    /// </summary>
    /// <param name="graph">Graphics</param>
    public override void DrawElement(Graphics graph)
    {
      if (_progressControl.Visible)
      {
        float fWidth = (float)_percentage;
        DrawProgressBar(graph, fWidth, _percentage);
      }
    }

    /// <summary>
    /// Disposes the object
    /// </summary>
    public override void Dispose()
    {
      if (_leftBitmap != null)
      {
        _leftBitmap.Dispose();
      }
      if (_midBitmap != null)
      {
        _midBitmap.Dispose();
      }
      if (_rightBitmap != null)
      {
        _rightBitmap.Dispose();
      }
      if (_backgroundBitmap != null)
      {
        _backgroundBitmap.Dispose();
      }
    }

    /// <summary>
    /// Checks, if an update for the element is needed
    /// </summary>
    /// <returns>true, if an update is needed</returns>
    protected override bool CheckElementSpecificForUpdate()
    {
      bool result = false;
      int newPercentage = GetPercentage();
      if (newPercentage != _percentage)
      {
        _percentage = newPercentage;
        result = true;
      }
      return result;
    }
    #endregion

    #region public overrides methods
    /// <summary>
    /// Draws the element for the cache status.
    /// </summary>
    /// <param name="graph">Graphics</param>
    /// <param name="cacheFill">Status of the cache</param>
    public override void DrawCacheStatus(Graphics graph, float cacheFill)
    {
      DrawProgressBar(graph, cacheFill, (int)cacheFill);
    }
    #endregion

    #region private methods
    /// <summary>
    /// Draws the progress bar with the given width and percentage
    /// </summary>
    /// <param name="graph">Graphics</param>
    /// <param name="fWidth">Width, depending on the percentage</param>
    /// <param name="percent">Percentage</param>
    private void DrawProgressBar(Graphics graph, float fWidth, int percent)
    {
      fWidth /= 100.0f;
      if (_backgroundBitmap != null)
      {
        graph.DrawImage(_backgroundBitmap, _progressControl.XPosition, _progressControl.YPosition, _progressControl.Width, _progressControl.Height);
      }
      if (_leftBitmap != null && _midBitmap != null && _rightBitmap != null)
      {
        int iWidthLeft = _leftBitmap.Width;
        int iHeightLeft = _leftBitmap.Height;
        int iWidthRight = _rightBitmap.Width;
        int iHeightRight = _rightBitmap.Height;
        GUIGraphicsContext.ScaleHorizontal(ref iWidthLeft);
        GUIGraphicsContext.ScaleHorizontal(ref iWidthRight);
        GUIGraphicsContext.ScaleVertical(ref iHeightLeft);
        GUIGraphicsContext.ScaleVertical(ref iHeightRight);
        //iHeight=20;
        int off = 12;
        GUIGraphicsContext.ScaleHorizontal(ref off);
        fWidth *= (float)(_progressControl.Width - 2 * off - iWidthLeft - iWidthRight);

        int iXPos = off + _progressControl.XPosition;

        int iYPos = _progressControl.YPosition + (_progressControl.Height - iHeightLeft) / 2;
        graph.DrawImage(_leftBitmap, iXPos, iYPos, iWidthLeft, iHeightLeft);

        iXPos += iWidthLeft;
        if (percent > 0 && (int)fWidth > 1)
        {
          graph.DrawImage(_midBitmap, iXPos, iYPos, (int)Math.Abs(fWidth), iHeightLeft);
          iXPos += (int)fWidth;
        }
        graph.DrawImage(_rightBitmap, iXPos, iYPos, iWidthRight, iHeightRight);
      }
    }

    /// <summary>
    /// Calculates the percentage of the control
    /// </summary>
    /// <returns></returns>
    private int GetPercentage()
    {
      int percent = 0;
      Int32.TryParse(GUIPropertyManager.Parse(_progressControl.Property), out percent);
      if (percent > 100)
        percent = 100;
      return percent;
    }
    #endregion
  }
}