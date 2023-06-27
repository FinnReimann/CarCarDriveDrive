public interface IObservee
{
    void Attach(Observer observer);
    void Detach(Observer observer);
    void NotifyObservers(CCDDEvents e);
}