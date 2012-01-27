using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iTunesLib;
using System.Runtime.InteropServices;

namespace iTunesNotify
{
    class iTunesCntrol : IDisposable
    {
        Form1 _form;
        private iTunesApp _itunes;
        public bool Checking { get; set; }

        public iTunesCntrol( Form1 form )
        {
            _form = form;
            Checking = Connection();
            _form.Connect = Checking;
            
        }

        ~iTunesCntrol()
        {
            this.Dispose();
        }

        public bool Connection()
        {
            if (_itunes == null){
                if (System.Diagnostics.Process.GetProcessesByName("iTunes").Count() > 0)
                {
                    _itunes = new iTunesApp();
                    Checking = true;

                    _form.Connect = Checking;
                    _form.Check = Checking;

                    _itunes.OnPlayerPlayEvent += new _IiTunesEvents_OnPlayerPlayEventEventHandler(_itunes_OnPlayerPlayEvent);
                    _itunes.OnQuittingEvent += new _IiTunesEvents_OnQuittingEventEventHandler(_itunes_OnQuittingEvent);

                    _form.notify("iTunesNotify", "success connection!");
                    return true;
                }
                else
                {
                    _form.notify("iTunesNotify", "no acitive iTunes!");
                }
            }
            return false;
        }

        public bool IsActive
	    {
	      get
	      {
	        return _itunes != null;
	      }
	    }
 
	    #region IDisposable Members
	 
	    /// <summary>
	    /// Performs application-defined tasks associated with freeing,
	    /// releasing, or resetting unmanaged resources.
	    /// </summary>
	    public void Dispose()
	    {
            if (_itunes != null)
            {
                // Release the com object by setting the reference counter to 0 
                Marshal.FinalReleaseComObject(_itunes);
                _itunes = null;
                _form.Connect = false;
                _form.notify("iTunesNotify", "disconnection!");
            } 
	    }
	 
	    #endregion

        // Event Handler 
        protected void _itunes_OnPlayerPlayEvent(object iTrack)
        {
            if (Checking)
            {
                string myArtist, myName;
                IITTrack myTrack = (IITTrack)iTrack;
                myArtist = myTrack.Artist;
                myName = myTrack.Name;

                _form.notify("iTunes", myArtist + "\n" + myName);
            }
        }

        protected void _itunes_OnQuittingEvent()
        {
            this.Dispose();
        }
    }
}
