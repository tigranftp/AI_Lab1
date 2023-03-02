﻿using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class PathNode : IComparable<PathNode>//: MonoBehaviour
{
    public bool walkable;           //  Свободна для перемещения
    public Vector2Int gridVec;
    public Vector3 worldPosition;   //  Позиция в глобальных координатах
    private GameObject objPrefab;   //  Шаблон объекта
    public GameObject body;         //  Объект для отрисовки
    
    private PathNode parentNode = null;               //  откуда пришли
    
    /// <summary>
    /// Родительская вершина - предшествующая текущей в пути от начальной к целевой
    /// </summary>
    public PathNode ParentNode
    {
        get => parentNode;
        set => SetParent(value);
    }

    public int x { get { return gridVec.x; } }
    public int y { get { return gridVec.y; } }

    private float distance = float.PositiveInfinity;  //  расстояние от начальной вершины

    /// <summary>
    /// Расстояние от начальной вершины до текущей (+infinity если ещё не развёртывали)
    /// </summary>
    public float Distance
    {
        get => distance;
        set => distance = value;
    }


    /// <summary>
    /// Устанавливаем родителя и обновляем расстояние от него до текущей вершины. Неоптимально - дважды расстояние считается
    /// </summary>
    /// <param name="parent"></param>
    private void SetParent(PathNode parent)
    {
        //  Указываем родителя
        parentNode = parent;
        //  Вычисляем расстояние
        if (parent != null)
            distance = parent.Distance + Dist(parent, this);
        else
            distance = float.PositiveInfinity;
    }

    /// <summary>
    /// Конструктор вершины
    /// </summary>
    /// <param name="_objPrefab">объект, который визуализируется в вершине</param>
    /// <param name="_walkable">проходима ли вершина</param>
    /// <param name="position">мировые координаты</param>
    public PathNode(GameObject _objPrefab, bool _walkable, Vector2Int grVec,  Vector3 position)
    {
        objPrefab = _objPrefab;
        walkable = _walkable;
        worldPosition = position;
        gridVec = grVec;
        body = GameObject.Instantiate(objPrefab, worldPosition, Quaternion.identity);
    }

    /// <summary>
    /// Расстояние между вершинами - разброс по высоте учитывается дополнительно
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public static float Dist(PathNode a, PathNode b)
    {
        return Vector3.Distance(a.body.transform.position, b.body.transform.position) + 40 * Mathf.Abs(a.body.transform.position.y - b.body.transform.position.y);
    }

    public static float Heuristic(PathNode a, PathNode b)
    {
        return Math.Abs(a.body.transform.position.x - b.body.transform.position.x) +
            Math.Abs(a.body.transform.position.y - b.body.transform.position.y) +
            Math.Abs(a.body.transform.position.z - b.body.transform.position.z);
    }

    /// <summary>
    /// Подсветить вершину - перекрасить в красный
    /// </summary>
    public void Illuminate()
    {
        body.GetComponent<Renderer>().material.color = Color.red;
    }
    public void FadeToYellow()
    {
        body.GetComponent<Renderer>().material.color = Color.yellow;
    }

    /// <summary>
    /// Снять подсветку с вершины - перекрасить в синий
    /// </summary>
    public void Fade()
    {
        body.GetComponent<Renderer>().material.color = Color.blue;
    }

    public int CompareTo(PathNode other)
    {
        if (this.distance > other.distance) {
            return 1;
        }

        if (this.distance == other.distance)
        {
            return 0;
        }
        return -1;
    }
}
