using System;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{
    [SerializeField] Chunk chunkPrefab;
    [SerializeField, Range(2f, 50f)] float chunkZTerm = 10f;
    [SerializeField, Range(4, 80)] int chunkLoads = 8;

    List<Chunk> _chunks;

    int top;
    GameMode gameMode;

    bool isMoving;
    float speed;


    public event Action onMoved;

    private void Update()
    {
        if (!isMoving) return;

        foreach (var chunk in _chunks)
        {
            isMoving &= chunk.MoveTowards(speed);
        }

        if (!isMoving)
        {
            foreach (var chunk in _chunks)
            {
                chunk.Stop();
            }

            onMoved?.Invoke();
        }
    }

    public void Init(GameMode mode)
    {
        top = 0;
        gameMode = mode;

        Chunk.Init(gameMode == GameMode.ANGRYMOM);

        if (_chunks != null)
        {
            foreach (var item in _chunks)
            {
                Destroy(item);
            }

            _chunks = new(chunkLoads);
        }

        if (_chunks == null)
        {
            _chunks = new(chunkLoads);
        }

        for (int i = 0; i < chunkLoads; i++)
        {
            var chunk = Instantiate(chunkPrefab, transform);

            chunk.transform.position += Vector3.forward * chunkZTerm * (i - 2);

            chunk.Init();

            _chunks.Add(chunk);
        }
    }

    [ContextMenu("MoveOneStep")]
    public void Move(float speed)
    {
        isMoving = true;
        var index = top;
        this.speed = speed;

        for (int i = 0; i < _chunks.Count; i++)
        {
            var nexIdx = (index + 1) % _chunks.Count;
            _chunks[nexIdx].SetDestination(_chunks[index].transform.position);
            index = nexIdx;
        }

        var reloadedChunk = _chunks[top++];

        reloadedChunk.transform.position = new Vector3(transform.position.x, transform.position.y, chunkZTerm * (_chunks.Count - 2));
        reloadedChunk.Init();

        top %= _chunks.Count;
    }
}