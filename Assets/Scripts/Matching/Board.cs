
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System.Linq;
using DG.Tweening;
using System;

public class Board : MonoBehaviour
{
    public static Board Instance { get; private set; }

    public Row[] rows;

    public Tile[,] Tiles { get; private set; }

    public int Width => Tiles.GetLength(dimension: 0);
    public int Height => Tiles.GetLength(dimension: 1);

    private List<Tile> _selected = new List<Tile>();

    private const float TweenDuration = 0.15f;

    private float gameScore = 0;

    public float _gameScore;

    bool isSwap = false;
    public void Awake() => Instance = this;

    private void Start()
    {
        Tiles = new Tile[rows.Max(selector: row => row.tiles.Length), rows.Length];

        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                var tile = rows[y].tiles[x];
                tile.x = x;
                tile.y = y;
                tile.Item = ItemDatabase.Items[UnityEngine.Random.Range(0, ItemDatabase.Items.Length)];
                Tiles[x, y] = tile;
            }
        }
    }

    public async void Select(Tile tile)
    {
        if(isSwap) { return; }
        if (!_selected.Contains(tile))
        {
            if (_selected.Count > 0)
            {
                if(Array.IndexOf(_selected[0].Neighbours, tile) != -1)
                {
                    _selected.Add(tile);
                }
                else if(Array.IndexOf(_selected[0].Neighbours, tile) == 0)
                {
                        return;
                }
            }
            else
            {
                _selected.Add(tile);
            }

        }

        if (_selected.Count < 2) return;
        
        await Swap(_selected[0], _selected[1]);

        if (CanPop())
        {
            Pop();
        }
        else
        {
            if (_selected[0] == _selected[1])
            {
                _selected.Clear();
            }
            await Swap(_selected[0], _selected[1]);
        }

        _selected.Clear();
    }

    public async Task Swap(Tile tile1, Tile tile2)
    {
        isSwap = true;
        var icon1 = tile1.icon;
        var icon2 = tile2.icon;

        var icon1Transform = icon1.transform;
        var icon2Transform = icon2.transform;

        var sequence = DOTween.Sequence();

        sequence.Join(icon1Transform.DOMove(icon2Transform.position, TweenDuration))
            .Join(icon2Transform.DOMove(icon1Transform.position, TweenDuration));

        await sequence.Play().AsyncWaitForCompletion();

        icon1Transform.SetParent(tile2.transform);
        icon2Transform.SetParent(tile1.transform);
        tile1.icon = icon2;
        tile2.icon = icon1;

        var tile1Item = tile1.Item;

        tile1.Item = tile2.Item;
        tile2.Item = tile1Item;
        isSwap = false;
    }
    
    private bool CanPop()
    {
        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                if (Tiles[x, y].GetConnectedTiles().Skip(1).Count() >1)
                    return true;
            }
        }
        Debug.Log("더이상 없음");
        return false;
    }

    private async void Pop()
    {
        
        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                var tile = Tiles[x, y];

                var connectedTiles = tile.GetConnectedTiles();

                if (connectedTiles.Skip(1).Count() < 2) continue;

                var defaultSequence = DOTween.Sequence();

                foreach (var connectedTile in connectedTiles)
                {
                    defaultSequence.Join(connectedTile.icon.transform.DOScale(Vector3.zero, TweenDuration));
                }
                await defaultSequence.Play()
                   .AsyncWaitForCompletion();

                gameScore += tile.Item.value * connectedTiles.Count;

                GameManager.Instance.coinMoney += (int)gameScore;

                gameScore = 0;

                var inflateSequence = DOTween.Sequence();

                foreach (var connectedTile in connectedTiles)
                {
                    connectedTile.Item = ItemDatabase.Items[UnityEngine.Random.Range(0, ItemDatabase.Items.Length)];

                    inflateSequence.Join(connectedTile.icon.transform.DOScale(Vector3.one, TweenDuration));
                }

                await inflateSequence.Play()
                    .AsyncWaitForCompletion();
            }
        }
    }

}


