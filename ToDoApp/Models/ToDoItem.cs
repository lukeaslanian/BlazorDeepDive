﻿namespace ToDoApp.Models;

public class ToDoItem
{
    private bool _isCompleted;
    public int Id { get; set; }
    public string Name { get; set; } = "";

    public bool IsCompleted
    {
        get => _isCompleted;
        set
        {
            _isCompleted = value;

            if (value) DateCompleted = DateTime.Now;
        }
    }

    public DateTime DateCompleted { get; set; }
}