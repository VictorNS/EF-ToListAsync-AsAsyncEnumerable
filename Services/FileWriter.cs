namespace EF_ToListAsync_AsAsyncEnumerable.Services;

public class FileWriterFactory
{
	public FileWriter CreateWriter()
	{
		return new FileWriter();
	}
}

public class FileWriter
{
	public string FullFileName { get; } = Path.Combine(AppContext.BaseDirectory, DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".log");

	public void Write(string value)
	{
		using (var streamWriter = new StreamWriter(FullFileName, true))
		{
			streamWriter.WriteLine(value);
		}
	}
}
