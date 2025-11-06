//
//  MyWidgetEntryView.swift
//  XCodeWidgetExample
//
//  Created by Toine Boer on 05/11/2025.
//

import WidgetKit
import SwiftUI
import AppIntents

struct MyWidgetEntryView : View {
    var entry: Provider.Entry

    var body: some View {
        VStack(spacing: 4) {
            
            HStack(spacing: 8) {
                Text(entry.favoriteConfiguredEmoji)
                Text("\(entry.count)")
                    .font(.largeTitle)
                    .fontWeight(.bold)
                    .multilineTextAlignment(.center)
                    .frame(maxWidth: .infinity)
                Text(entry.favoriteConfiguredEmoji)
            }
            
            HStack(spacing: 8) {
                Button(intent: DecrementCounterIntent()) {
                    Text("-")
                        .font(.system(size: 40))
                        .frame(maxWidth: .infinity, minHeight: 44)
                        .cornerRadius(8)
                }
                
                Button(intent: IncrementCounterIntent()) {
                    Text("+")
                        .font(.system(size: 40))
                        .frame(maxWidth: .infinity, minHeight: 44)
                        .cornerRadius(8)
                }
            }
            
            Text(entry.message)
                .multilineTextAlignment(.center)
                .frame(maxWidth: .infinity)
        }
        .padding()
        .widgetURL(URL(string: entry.widgetUrl))
    }
}
