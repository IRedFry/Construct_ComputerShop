namespace BLL 
{ 
    public class Salary
    {
        public Salary(decimal wage, int exp_years, int order_count)
        {
            base_salary = wage;
            this.exp_years = exp_years;
            this.order_count = order_count;
        }

        public decimal base_salary { get; set; }
        public decimal exp_years { get; set; }
        public decimal order_count { get; set; }

        public decimal GetWage()
        {
            return base_salary * (1 + (exp_years) / 20 + order_count / 500);
        }
    }
}
