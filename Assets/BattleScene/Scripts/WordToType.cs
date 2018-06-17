public class WordToType
{
    private WordDisplay _wordDisplay;

    private string _word;
    private int _index;
    private EmotionMenager _emotionMenager;

    public WordToType(string word, WordDisplay wordDisplay)
    {
        _word = word;
        _index = 0;

        _wordDisplay = wordDisplay;
        wordDisplay.SetWord(_word);
    }

    public void TypeLetter(char letter)
    {
        if (_word[_index] == letter)
        {
            _index++;
            EmotionMenager.GetInstance().HandleEvent(EmotionEventType.TYPE_CORRECT_SIGN);
            _wordDisplay.RemoveLetter();
        } else
        {
            EmotionMenager.GetInstance().HandleEvent(EmotionEventType.MISSPELL);
        }
    }

    public bool IsWordTyped()
    {
        var isTyped = _index == _word.Length;

        if(isTyped)
            _wordDisplay.RemoveWord();

        return isTyped;
    }

    public void RemoveWord()
    {
        _wordDisplay.RemoveWord();
    }
}