using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SingletonsPreloaders
{
    public class SpritesHolder : UnitySingletonBase<SpritesHolder>
    {
        [System.Serializable]
        public class NamedSprite
        {
            public string Name;
            public Sprite Sprite;
        }

        [SerializeField] private NamedSprite[] _namedSprites;
        private Dictionary<string, Sprite> _namedSpritesDict = new Dictionary<string, Sprite>();

        protected override SpritesHolder GetInstance() => this;

        private void Start()
        {
            InitDict();
        }

        private void InitDict()
        {
            for(int i = 0; i < _namedSprites.Length; i++)
            {
                var namedSprite = _namedSprites[i];
                var key = namedSprite.Name;
                if(!_namedSpritesDict.ContainsKey(key))
                {
                    _namedSpritesDict.Add(key, namedSprite.Sprite);
                }
            }
        }

        public Sprite GetSpriteByKey(string spriteKey)
        {
            Sprite sprite = null;
            if(_namedSpritesDict.ContainsKey(spriteKey))
            {
                sprite = _namedSpritesDict[spriteKey];
            }
            else
            {
                Debug.LogError($"Cannot get sprite with key: {spriteKey} !");
            }

            return sprite;
        }
    }

}
