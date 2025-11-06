//
//  SilentNotificationService.swift
//  XCodeWidgetExample
//
//  Created by Toine Boer on 05/11/2025.
//

import WidgetKit
import SwiftUI

class SilentNotificationService {
    func sendDataWithoutOpeningApp() async throws {
        // Simulate a network call with 100ms delay
        try await Task.sleep(nanoseconds: 100_000_000) 
        
        // Simulate that the call has happened
        print("📡 Silent data sent successfully")
    }
}