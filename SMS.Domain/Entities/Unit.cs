using SMS.Shared.Guards;

namespace SMS.Domain.Entities
{
    public sealed class Unit
    {
        private int _unitId;
        private string _unitName = null!;
        private string _symbol = null!;

        public int UnitId
        {
            get => _unitId;
            set
            {
                NumericGuard.AgainstInvalidId(value);
                _unitId = value;
            }
        }

        public string UnitName
        {
            get => _unitName;
            set
            {
                StringGuard.AgainstNullOrWhiteSpace(value, nameof(UnitName));
                StringGuard.AgainstExcessiveLength(value, 20, nameof(UnitName));
                _unitName = value.Trim();
            }
        }

        public string Symbol
        {
            get => _symbol;
            set
            {
                StringGuard.AgainstNullOrWhiteSpace(value, nameof(Symbol));
                StringGuard.AgainstExcessiveLength(value, 10, nameof(Symbol));
                _symbol = value.Trim();
            }
        }

        public bool IsDecimal { get; set; }

        public Unit()
        {
        }

        public Unit(string unitName, string symbol, bool isDecimal)
        {
            UnitName = unitName;
            Symbol = symbol;
            IsDecimal = isDecimal;
        }

        public Unit(int unitId, string unitName, string symbol, bool isDecimal)
            : this(unitName, symbol, isDecimal)
        {
            UnitId = unitId;
        }
    }
}