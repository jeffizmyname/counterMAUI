using System.Windows.Input;
using System.Collections.ObjectModel;
using System.ComponentModel;
using counter.Models;
using System.Diagnostics;
using Google.Cloud.Firestore;

namespace counter.ViewModels
{
    public class CounterViewModel
    {
        public ObservableCollection<Counter> Counters { get; } = new ObservableCollection<Counter>();
        FirestoreDb db;

        public ICommand IncrementCommand { get; }
        public ICommand DecrementCommand { get; }
        public ICommand ResetCommand { get; }
        public ICommand DeleteCommand { get; }

        public CounterViewModel(FirestoreDb databaseRef)
        {
            IncrementCommand = new Command<Counter>(Increment);
            DecrementCommand = new Command<Counter>(Decrement);
            ResetCommand = new Command<Counter>(Reset);
            DeleteCommand = new Command<Counter>(Delete);
            db = databaseRef;
            LoadCountersFromDb();
        }

        public async void AddCounter(string counterName, int baseValue, Color selectedColor)
        {
            string id = await AddCounterToDb(counterName, baseValue, selectedColor);
            Counters.Add(new Counter(id, counterName, selectedColor, baseValue));
        }

        private void Increment(Counter counter)
        {
            counter.Value++;
            ChganeValueInDb(counter.id, counter.Value);
        }

        private void Decrement(Counter counter)
        {
            counter.Value--;
            ChganeValueInDb(counter.id, counter.Value);
        }

        private void Reset(Counter counter)
        {
            counter.Value = counter.defaultVal;
            ChganeValueInDb(counter.id, counter.Value);
        }

        private void Delete(Counter counter)
        {
            Counters.Remove(counter);
            DeleteCounterFromDb(counter.id);
        }

        private async void ChganeValueInDb(string id, int val) 
        { 
            DocumentReference docRef = db.Collection("counters").Document(id);
            Dictionary<string, object> data = new Dictionary<string, object>() 
            {
                {"Value", val}
            };

            DocumentSnapshot snap = await docRef.GetSnapshotAsync();
            if(snap.Exists)
            {
                await docRef.UpdateAsync(data);     
            }

        }
        private async Task<string> AddCounterToDb(string counterName, int baseValue, Color color)
        {
            CollectionReference coll = db.Collection("counters");
            Dictionary<string, object> data = new Dictionary<string, object>()
        {
            {"Name", counterName},
            {"Value",  baseValue},
            {"BaseValue", baseValue},
            {"Color", color.ToArgbHex()}
        };
            DocumentReference docRef = await coll.AddAsync(data);
            string documentId = docRef.Id;
            return documentId;
        }

        private void DeleteCounterFromDb(string id)
        {
            DocumentReference docRef = db.Collection("counters").Document(id);
            docRef.DeleteAsync();
        }

        private async void LoadCountersFromDb()
        {
            Query Qref = db.Collection("counters");
            QuerySnapshot snap = await Qref.GetSnapshotAsync();

            foreach (DocumentSnapshot docsnap in snap)
            {
                if (docsnap.Exists)
                {
                    Dictionary<string, object> counterDic = docsnap.ToDictionary();
                    Counter counter = new Counter(counterDic)
                    {
                        id = docsnap.Id,
                    };
                    Counters.Add(counter);
                }
            }
        }
    }
}
