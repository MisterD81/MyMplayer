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
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using MediaPortal.Configuration;

namespace MPlayer.ConfigurationPanel {
  /// <summary>
  /// This class represents the video section of the configuration
  /// </summary>
  public partial class VideoSection : UserControl {

    #region ctor
    /// <summary>
    /// Constructor, which initilizes the control
    /// </summary>
    public VideoSection() {
      InitializeComponent();
    }
    #endregion

    #region configuration methods
    /// <summary>
    /// Loads the configuration of this section
    /// </summary>
    public void LoadConfiguration() {
      using (MediaPortal.Profile.Settings xmlreader = new MediaPortal.Profile.Settings(Config.GetFile(Config.Dir.Config, "MediaPortal.xml"))) {
        videoOutputDriver.SelectedIndex = xmlreader.GetValueAsInt("mplayer", "videoOutputDriver", (int)VideoOutputDriver.DirectX);
        postProcessing.SelectedIndex = xmlreader.GetValueAsInt("mplayer", "postProcessing", (int)PostProcessing.Maximum);
        aspectRatio.SelectedIndex = xmlreader.GetValueAsInt("mplayer", "aspectRatio", (int)AspectRatio.Automatic);
        deinterlace.SelectedIndex = xmlreader.GetValueAsInt("mplayer", "deinterlace", (int)Deinterlace.Adaptive);
        noiseDenoise.SelectedIndex = xmlreader.GetValueAsInt("mplayer", "noise", (int)NoiseDenoise.Nothing);
        framedrop.Checked = xmlreader.GetValueAsBool("mplayer", "framedrop", false);
        directRendering.Checked = xmlreader.GetValueAsBool("mplayer", "directRendering", true);
        doubleBuffering.Checked = xmlreader.GetValueAsBool("mplayer", "doubleBuffering", true);
      }
    }

    /// <summary>
    /// Stores the configuration of this section
    /// </summary>
    public void SaveConfiguration() {
      using (MediaPortal.Profile.Settings xmlWriter = new MediaPortal.Profile.Settings(Config.GetFile(Config.Dir.Config, "MediaPortal.xml"))) {
        xmlWriter.SetValue("mplayer", "videoOutputDriver", videoOutputDriver.SelectedIndex);
        xmlWriter.SetValue("mplayer", "postProcessing", postProcessing.SelectedIndex);
        xmlWriter.SetValue("mplayer", "aspectRatio", aspectRatio.SelectedIndex);
        xmlWriter.SetValue("mplayer", "deinterlace", deinterlace.SelectedIndex);
        xmlWriter.SetValue("mplayer", "noise", noiseDenoise.SelectedIndex);
        xmlWriter.SetValueAsBool("mplayer", "framedrop", framedrop.Checked);
        xmlWriter.SetValueAsBool("mplayer", "directRendering", directRendering.Checked);
        xmlWriter.SetValueAsBool("mplayer", "doubleBuffering", doubleBuffering.Checked);
      }
    }
    #endregion

    #region event handling
    /// <summary>
    /// Handles the video output driver changed
    /// </summary>
    /// <param name="sender">Sender object</param>
    /// <param name="e">Event Arguments</param>
    private void videoOutputDriver_SelectedIndexChanged(object sender, EventArgs e) {
      directRendering.Enabled = videoOutputDriver.SelectedIndex != (int)VideoOutputDriver.OpenGL;
    }
    #endregion
  }
}