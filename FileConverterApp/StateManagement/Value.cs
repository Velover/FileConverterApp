namespace FileConverterApp.StateManagement
{
	internal class Value<T>
	{
		public delegate void OnChangedEventHandler(object sender, ValueChangedEventArgs<T> args);
		public event OnChangedEventHandler OnChanged;
		T value_;
		public Value(T start_value)
		{
			value_ = start_value;
		}

		private ValueChangedEventArgs<T> GetEventArgsWithCurrentValue() => new ValueChangedEventArgs<T>(value_, value_);

		public void TriggerFor(OnChangedEventHandler handler)
		{
			handler(this, GetEventArgsWithCurrentValue());
		}
		public T GetValue() => value_;
		public void SetValue(T value)
		{
			var old_value = value_;
			value_ = value;
			if (OnChanged is not null)
				OnChanged(this, new ValueChangedEventArgs<T>(new_value: value_, old_value: old_value));
		}
	}

	class ValueChangedEventArgs<T> : EventArgs
	{
		public T Value { get; private set; }
		public T OldValue { get; private set; }
		public ValueChangedEventArgs(T new_value, T old_value)
		{
			Value = new_value;
			OldValue = old_value;
		}
	}
}
