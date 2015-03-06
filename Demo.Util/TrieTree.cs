using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Demo.Util
{
    public class TermEntity
    {
        public string termValue;

        public double termWeight;
    }

    class TrieNode
    {
        public bool isStr; //记录此处是否构成一个串。

        public TrieNode[] nextNode = new TrieNode[26];//指向各个子树的指针,下标0-25代表26字符

        public IList<TermEntity> valueList = new List<TermEntity>();

        public TrieNode()
        {

        }
    }

    public class TrieTree
    {
        private TrieNode root;
        private List<TermEntity> _suggestWords = new List<TermEntity>();

        public TrieTree()
        {
            root = new TrieNode();
        }

        public void Insert(string word, string termword)
        {
            TrieNode location = root;

            foreach (char c in word.ToLower().ToCharArray())
            {
                if (location.nextNode[c - 'a'] == null)//不存在则建立
                {
                    TrieNode tmp = new TrieNode();
                    location.nextNode[c - 'a'] = tmp;
                }

                location = location.nextNode[c - 'a'];//每插入一步，相当于有一个新串经过，指针要向下移动
            }
            location.isStr = true;
            TermEntity entity = new TermEntity();
            entity.termValue = termword;
            entity.termWeight = 0.6;
            location.valueList.Add(entity);
        }

        public List<TermEntity> Search(string word)
        {
            List<TermEntity> suggestWords = new List<TermEntity>();
            TrieNode location = root;
            word = word.ToLower();
            foreach (char c in word.ToCharArray())
            {
                if (location != null)
                {
                    location = location.nextNode[c - 'a'];
                }
            }

            if (location != null)
            {
                if (location.isStr)
                {                    
                    _suggestWords.AddRange(location.valueList);
                }

                findSuggest(location, word);

                if (_suggestWords.Count > 0)
                {
                    suggestWords.AddRange(_suggestWords);
                    _suggestWords.Clear();
                }

            }
            return suggestWords;
        }

        private void findSuggest(TrieNode location, string srcWord)
        {

            for (int i = 0; i < 26; i++)
            {
                string newWord = string.Empty;

                if (location.nextNode[i] != null)
                {
                    int append = Convert.ToChar(i) + 'a';
                    newWord = srcWord + (char)append;

                    if (location.nextNode[i].isStr)
                    {
                        _suggestWords.AddRange(location.nextNode[i].valueList);
                    }

                    findSuggest(location.nextNode[i], newWord);
                }
            }
            return;
        }
    }
}
