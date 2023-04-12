using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

/*
 * This module satisfies:
 *   - Functional requirement 2.3
 */
public class PathRequest : MonoBehaviour
{
    // Queue to store requests to be made
    Queue<StructPathRequest> pathRequestQueue = new Queue<StructPathRequest>();
    StructPathRequest currentPathRequest;  // The current path request

    static PathRequest instance;
    AStar aStar;  // To run A*

    bool isProcessingPath;  // To check if processing path

    void Awake()  // Get Componenets before start
    {
        instance = this;
        aStar = GetComponent<AStar>();
    }
    public static void RequestPath(Vector2 pathStart, Vector2 pathEnd, Action<Vector2[], bool> callback)
    {
        // Get request 
        StructPathRequest newRequest = new StructPathRequest(pathStart, pathEnd, callback);
        instance.pathRequestQueue.Enqueue(newRequest);  // Add it to the queue
        instance.TryProcessNext();  // Try to process request
    }

    void TryProcessNext()
    {
        if (!isProcessingPath && pathRequestQueue.Count > 0)  // Process if turn
        {
            currentPathRequest = pathRequestQueue.Dequeue();  // Remove request from queue
            isProcessingPath = true;

            // Run A* to get path
            aStar.startFindPath(currentPathRequest.pathStart, currentPathRequest.pathEnd);
        }
    }

    public void FinishProcessingPath(Vector2[] path, bool success)
    {
        currentPathRequest.callback(path, success);  // Return success if we found a path
        isProcessingPath = false;
        TryProcessNext();  // Process next node if it exists
    }

    struct StructPathRequest  // Struct to store information about a request to A*
    {
        public Vector2 pathStart;
        public Vector2 pathEnd;
        public Action<Vector2[], bool> callback;

        public StructPathRequest(Vector2 _start, Vector2 _end, Action<Vector2[], bool> _callback)
        {
            pathStart = _start;
            pathEnd = _end;
            callback = _callback;
        }
    }
}
