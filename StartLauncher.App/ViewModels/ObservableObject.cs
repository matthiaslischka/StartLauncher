using System.ComponentModel;

namespace StartLauncher.App.ViewModels
{
  public class ObservableObject
  {
    public event PropertyChangedEventHandler PropertyChanged;

    protected void RaisePropertyChangedEvent(string propertyName)
    {
      var handler = PropertyChanged;
      handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
  }
}