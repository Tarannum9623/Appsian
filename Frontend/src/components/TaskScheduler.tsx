// components/TaskScheduler.tsx
import React, { useState } from 'react';
import { tasksAPI } from '../services/api';
import { ScheduledTask, ScheduleResponse } from '../types';

interface TaskSchedulerProps {
  projectId: number;
}

const TaskScheduler: React.FC<TaskSchedulerProps> = ({ projectId }) => {
  const [tasks, setTasks] = useState<ScheduledTask[]>([
    { title: '', estimatedHours: 0, dueDate: '', dependencies: [] }
  ]);
  const [schedule, setSchedule] = useState<ScheduleResponse | null>(null);
  const [loading, setLoading] = useState(false);

  const addTask = () => {
    setTasks([...tasks, { title: '', estimatedHours: 0, dueDate: '', dependencies: [] }]);
  };

  const updateTask = (index: number, field: string, value: any) => {
    const updatedTasks = [...tasks];
    updatedTasks[index] = { ...updatedTasks[index], [field]: value };
    setTasks(updatedTasks);
  };

  const removeTask = (index: number) => {
    setTasks(tasks.filter((_, i) => i !== index));
  };

  const generateSchedule = async () => {
    setLoading(true);
    try {
      const response = await tasksAPI.schedule(projectId, { tasks });
      setSchedule(response.data);
    } catch (error) {
      console.error('Failed to generate schedule');
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="bg-white rounded-lg shadow-md p-6 mb-6">
      <h3 className="text-xl font-semibold mb-4">Smart Task Scheduler</h3>
      
      <div className="space-y-4">
        {tasks.map((task, index) => (
          <div key={index} className="border border-gray-200 rounded-lg p-4">
            <div className="grid grid-cols-1 md:grid-cols-2 gap-4 mb-4">
              <div>
                <label className="block text-sm font-medium text-gray-700">Task Title</label>
                <input
                  type="text"
                  className="mt-1 block w-full border border-gray-300 rounded-md px-3 py-2"
                  value={task.title}
                  onChange={(e) => updateTask(index, 'title', e.target.value)}
                />
              </div>
              <div>
                <label className="block text-sm font-medium text-gray-700">Estimated Hours</label>
                <input
                  type="number"
                  className="mt-1 block w-full border border-gray-300 rounded-md px-3 py-2"
                  value={task.estimatedHours}
                  onChange={(e) => updateTask(index, 'estimatedHours', parseInt(e.target.value))}
                />
              </div>
              <div>
                <label className="block text-sm font-medium text-gray-700">Due Date</label>
                <input
                  type="date"
                  className="mt-1 block w-full border border-gray-300 rounded-md px-3 py-2"
                  value={task.dueDate}
                  onChange={(e) => updateTask(index, 'dueDate', e.target.value)}
                />
              </div>
              <div>
                <label className="block text-sm font-medium text-gray-700">
                  Dependencies (comma-separated task titles)
                </label>
                <input
                  type="text"
                  className="mt-1 block w-full border border-gray-300 rounded-md px-3 py-2"
                  placeholder="Task A, Task B"
                  value={task.dependencies.join(', ')}
                  onChange={(e) => updateTask(index, 'dependencies', e.target.value.split(',').map(s => s.trim()))}
                />
              </div>
            </div>
            {tasks.length > 1 && (
              <button
                onClick={() => removeTask(index)}
                className="text-red-600 hover:text-red-800 text-sm"
              >
                Remove Task
              </button>
            )}
          </div>
        ))}
      </div>

      <div className="flex space-x-2 mt-4">
        <button
          onClick={addTask}
          className="bg-gray-600 text-white px-4 py-2 rounded-md hover:bg-gray-700"
        >
          Add Task
        </button>
        <button
          onClick={generateSchedule}
          disabled={loading || tasks.some(t => !t.title)}
          className="bg-green-600 text-white px-4 py-2 rounded-md hover:bg-green-700 disabled:opacity-50"
        >
          {loading ? 'Generating...' : 'Generate Schedule'}
        </button>
      </div>

      {schedule && (
        <div className="mt-6 p-4 bg-green-50 rounded-lg">
          <h4 className="font-semibold text-green-800 mb-2">Recommended Schedule:</h4>
          <ol className="list-decimal list-inside space-y-1">
            {schedule.recommendedOrder.map((task, index) => (
              <li key={task} className="text-green-700">
                {task}
                {schedule.startDates[task] && (
                  <span className="text-green-600 text-sm ml-2">
                    (Start: {new Date(schedule.startDates[task]).toLocaleDateString()})
                  </span>
                )}
              </li>
            ))}
          </ol>
        </div>
      )}
    </div>
  );
};

export default TaskScheduler;