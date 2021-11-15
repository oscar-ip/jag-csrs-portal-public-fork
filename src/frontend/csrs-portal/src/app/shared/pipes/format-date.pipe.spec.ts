import { FormatDatePipe } from './format-date.pipe';

describe('DatePipe', () => {
  it('create an instance', () => {
    const pipe = new FormatDatePipe();
    expect(pipe).toBeTruthy();
  });

  it('should handle null', () => {
    const pipe = new FormatDatePipe();
    const after = pipe.transform(null);
    expect(after).toBeNull();
  });
});
