namespace AmisduMalade.Services
{
    public interface IReportService
    {
        Task<object> GetMonthlyReportAsync(int year, int month);
        Task<object> GetAnnualReportAsync(int year);
    }
}