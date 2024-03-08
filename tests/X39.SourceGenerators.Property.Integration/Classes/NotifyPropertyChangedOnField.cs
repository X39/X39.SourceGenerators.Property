namespace TestNamespace;

public partial class NotifyPropertyChangedOnField : System.ComponentModel.INotifyPropertyChanged
{
    public event System.ComponentModel.PropertyChangedEventHandler? PropertyChanged;
                                                                
    [NotifyPropertyChanged]
    private float _field;
}